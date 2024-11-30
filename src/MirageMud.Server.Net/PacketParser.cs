using Microsoft.Extensions.Logging;

namespace MirageMud.Server.Net;

internal sealed class PacketParser<TClientState>(ILogger logger) where TClientState : Enum
{
    private const int BufferSize = 4096;

    private readonly byte[] _buffer = new byte[BufferSize];
    private readonly Dictionary<int, Func<ReadOnlySpan<byte>, Task>> _handlers = [];
    private int _bytesReceived;

    private static int UniqueId(TClientState state, int packetId)
    {
        return ((int) (object) state & 0xFFFF) << 16 | (packetId & 0xFFFF);
    }

    public void Bind<TPacket>(TClientState state, int packetId, Func<TPacket, Task> handler) where TPacket : IPacket<TPacket>
    {
        _handlers[UniqueId(state, packetId)] = bytes =>
        {
            var packetReader = new PacketReader(bytes);
            var packet = TPacket.ReadFrom(packetReader);

            return handler.Invoke(packet);
        };
    }

    public void Bind(TClientState state, int packetId, Func<Task> handler)
    {
        _handlers[UniqueId(state, packetId)] = _ => handler.Invoke();
    }

    public async Task Parse(TClientState state, byte[] bytes, int offset, int count)
    {
        var bytesAvailable = _buffer.Length - _bytesReceived;
        if (count > bytesAvailable)
        {
            throw new Exception("Buffer overflow");
        }

        bytes.AsSpan(offset, count).CopyTo(_buffer.AsSpan(_bytesReceived));

        _bytesReceived += count;

        await ProcessBytes(state);
    }

    private async Task ProcessBytes(TClientState state)
    {
        var bytesProcessed = 0;
        var bytesLeft = _bytesReceived;

        while (bytesLeft - bytesProcessed >= 2)
        {
            var len = BitConverter.ToInt16(_buffer, bytesProcessed);

            bytesLeft -= 2;
            if (bytesLeft < len)
            {
                break;
            }

            await ProcessPacket(state, _buffer, bytesProcessed + 2, len);

            bytesProcessed += 2 + len;
            bytesLeft -= len;
        }

        if (bytesProcessed == 0)
        {
            return;
        }

        if (bytesLeft > 0)
        {
            Buffer.BlockCopy(_buffer, bytesProcessed, _buffer, 0, bytesLeft);
        }

        _bytesReceived -= bytesProcessed;
    }

    private async Task ProcessPacket(TClientState state, byte[] bytes, int offset, int count)
    {
        var packet = bytes.AsSpan(offset, count);
        if (packet.Length < 2)
        {
            return;
        }

        var packetId = BitConverter.ToInt16(packet);
        var packetIdUnique = UniqueId(state, packetId);

        if (_handlers.TryGetValue(packetIdUnique, out var handler))
        {
            await handler.Invoke(packet[2..]);
        }
        else
        {
            logger.LogInformation("Received a unknown packet with ID {PacketId}", packetId);
        }
    }
}