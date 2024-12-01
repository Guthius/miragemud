using System.Buffers;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using MirageMud.Server.Net.Extensions;

namespace MirageMud.Server.Net;

public abstract class Connection<TClient, TClientState>(ILogger logger)
    : IConnection where TClient : Connection<TClient, TClientState> where TClientState : struct, Enum
{
    private const int BufferSize = 4096;
    private const int PacketHeaderSize = 2;

    private sealed record ConnectionState(Socket Socket, string SocketAddr, byte[] ReceiveBuffer);

    private readonly PacketParser<TClientState> _parser = new(logger);
    private readonly Lock _packetBufferLock = new();
    private readonly BufferedPacketWriter _packetBuffer = new(BufferSize);
    private readonly ConcurrentQueue<byte[]> _sendBuffer = new();
    private Server<TClient, TClientState>? _server;
    private bool _closed;
    private Socket? _socket;
    private bool _sending;
    private TClientState _state;

    public int Id { get; private set; }

    protected void SetState(TClientState state)
    {
        _state = state;
    }

    protected sealed class StateBinder(Connection<TClient, TClientState> connection, TClientState state)
    {
        public void Bind<TPacket>(int packetId, Func<TPacket, Task> handler) where TPacket : IPacket<TPacket>
        {
            connection.Bind(state, packetId, handler);
        }

        public void Bind(int packetId, Func<Task> handler)
        {
            connection.Bind(state, packetId, handler);
        }
    }

    protected void When(TClientState state, Action<StateBinder> action)
    {
        var stateBinder = new StateBinder(this, state);

        action(stateBinder);
    }

    protected void Bind<TPacket>(TClientState state, int packetId, Func<TPacket, Task> handler) where TPacket : IPacket<TPacket>
    {
        _parser.Bind(state, packetId, handler);
    }

    protected void Bind(TClientState state, int packetId, Func<Task> handler)
    {
        _parser.Bind(state, packetId, handler);
    }

    public void Send(IPacket packet)
    {
        if (_socket is null)
        {
            return;
        }

        byte[] bytes;

        lock (_packetBufferLock)
        {
            _packetBuffer.Reset();

            packet.WriteTo(_packetBuffer);

            bytes = _packetBuffer.ToPacket();
            if (bytes.Length == PacketHeaderSize)
            {
                return;
            }
        }

        _sendBuffer.Enqueue(bytes);

        SendNow(_socket);
    }

    protected void SendToAll(IPacket packet, Func<TClient, bool> predicate)
    {
        if (_server is null)
        {
            throw new InvalidOperationException("The connection is not running");
        }

        _server.SendTo(packet, predicate);
    }

    public void Disconnect()
    {
        Close();
    }

    internal void Run(Server<TClient, TClientState> server, int id, Socket socket)
    {
        _server = server;
        _socket = socket;
        _closed = false;
        
        Id = id;
        
        var connectionState = new ConnectionState(socket, socket.GetRemoteIp(), ArrayPool<byte>.Shared.Rent(BufferSize));

        logger.LogInformation("[{ConnectionId}] Connection established with {SocketAddr}", Id, connectionState.SocketAddr);

        OnConnected();

        socket.BeginReceive(
            connectionState.ReceiveBuffer,
            0, BufferSize,
            SocketFlags.None,
            ReceiveEnd,
            connectionState);
    }

    private void ReceiveEnd(IAsyncResult ar)
    {
        var state = (ConnectionState) ar.AsyncState!;

        try
        {
            var bytesReceived = state.Socket.EndReceive(ar);
            if (bytesReceived == 0)
            {
                Close();

                return;
            }

            var task = _parser.Parse(_state, state.ReceiveBuffer, 0, bytesReceived);

            task.Wait();

            state.Socket.BeginReceive(
                state.ReceiveBuffer,
                0, BufferSize,
                SocketFlags.None,
                ReceiveEnd,
                state);
        }
        catch (SocketException)
        {
            logger.LogWarning("[{ConnectionId}] Connection with {SocketAddr} has been lost", Id, state.SocketAddr);

            Close();
        }
    }

    private sealed record SendState
    {
        public required Socket Socket { get; init; }
        public required byte[] Bytes { get; init; }
        public required int BytesToSend { get; init; }
        public int BytesSent { get; set; }
    }

    private void SendNow(Socket socket)
    {
        var sending = Interlocked.Exchange(ref _sending, true);
        if (sending)
        {
            return;
        }

        SendNextPacketFromQueue(socket);
    }

    private void SendNextPacketFromQueue(Socket socket)
    {
        while (true)
        {
            var dataAvailable = _sendBuffer.TryDequeue(out var data);
            if (!dataAvailable)
            {
                Interlocked.Exchange(ref _sending, false);
                return;
            }

            if (data is null)
            {
                continue;
            }

            socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendEnd, new SendState
            {
                Socket = socket,
                Bytes = data,
                BytesToSend = data.Length,
                BytesSent = 0
            });

            break;
        }
    }

    private void SendEnd(IAsyncResult ar)
    {
        var state = (SendState) ar.AsyncState!;

        try
        {
            var bytesSent = state.Socket.EndSend(ar);
            if (bytesSent == 0)
            {
                return;
            }

            state.BytesSent += bytesSent;
            if (state.BytesSent >= state.BytesToSend)
            {
                SendNextPacketFromQueue(state.Socket);
                return;
            }

            state.Socket.BeginSend(state.Bytes, state.BytesSent, state.BytesToSend - state.BytesSent, SocketFlags.None, SendEnd, state);
        }
        catch (SocketException)
        {
        }
    }

    protected virtual void OnConnected()
    {
    }

    protected virtual void OnDisconnected()
    {
    }

    private void Close()
    {
        var closed = Interlocked.Exchange(ref _closed, true);
        if (closed)
        {
            return;
        }

        if (_socket is not null)
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                logger.LogInformation("[{ConnectionId}] Connection with {SocketAddr} has been closed", Id, _socket.GetRemoteIp());

                _socket.Close();
                _socket = null;
            }
        }

        OnDisconnected();

        _server?.Unregister(Id);
        _server = null;
    }
}