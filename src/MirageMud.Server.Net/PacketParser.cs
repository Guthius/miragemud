using Microsoft.Extensions.Logging;

namespace MirageMud.Server.Net;


internal sealed class PacketParser(ILogger logger)
{
    private const int BufferSize = 4096;

    private readonly byte[] _buffer = new byte[BufferSize];
    private readonly Dictionary<int, Action<ReadOnlySpan<byte>>> _handlers = [];
    private int _count;

    public void Bind<TPacket>(int packetId, Action<TPacket> handler) where TPacket : IPacket<TPacket>
    {
        _handlers[packetId] = bytes =>
        {
            var packetReader = new PacketReader(bytes);
            var packet = TPacket.ReadFrom(packetReader);

            handler.Invoke(packet);
        };
    }

    public void Parse(ReadOnlySpan<byte> bytes)
    {
        var bytesAvailable = _buffer.Length - _count;
        if (bytes.Length > bytesAvailable)
        {
            throw new Exception("Buffer overflow");
        }

        bytes.CopyTo(_buffer.AsSpan(_count));

        _count += bytes.Length;

        ProcessBytes();
    }

    private void ProcessBytes()
    {
        var bytesProcessed = 0;

        var buffer = _buffer.AsSpan(0, _count);
        while (buffer.Length >= 2)
        {
            var len = BitConverter.ToInt16(buffer);
            buffer = buffer[2..];

            if (buffer.Length < len)
            {
                break;
            }

            ProcessPacket(buffer[..len]);

            buffer = buffer[len..];
            bytesProcessed += 2 + len;
        }

        if (bytesProcessed == 0)
        {
            return;
        }

        var bytesLeft = _count - bytesProcessed;
        if (bytesLeft > 0)
        {
            Buffer.BlockCopy(_buffer, bytesProcessed, _buffer, 0, bytesLeft);
        }

        _count -= bytesProcessed;
    }

    private void ProcessPacket(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length < 2)
        {
            return;
        }

        var packetId = BitConverter.ToInt16(bytes);

        if (_handlers.TryGetValue(packetId, out var handler))
        {
            handler.Invoke(bytes[2..]);
        }
        else
        {
            logger.LogInformation("Received a unknown packet with ID {PacketId}", packetId);
        }
    }
}