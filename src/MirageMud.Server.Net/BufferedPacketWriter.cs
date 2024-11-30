using System.Text;

namespace MirageMud.Server.Net;

internal sealed class BufferedPacketWriter(int bufferSize) : IPacketWriter
{
    private readonly byte[] _buffer = new byte[bufferSize];
    private int _pos;

    public void Reset()
    {
        _pos = 0;
    }

    private void EnsureSpace(int size)
    {
        if (_pos + size > _buffer.Length)
        {
            throw new InvalidOperationException("Not enough space in buffer");
        }
    }

    public byte[] ToPacket()
    {
        var bytes = new byte[2 + _pos];
        BitConverter.TryWriteBytes(bytes.AsSpan(0), (ushort) _pos);
        _buffer.AsSpan(0, _pos).CopyTo(bytes.AsSpan(2));
        return bytes;
    }

    public void WriteByte(byte value)
    {
        EnsureSpace(1);
        _buffer[_pos++] = value;
    }

    public void WriteBytes(ReadOnlySpan<byte> bytes)
    {
        EnsureSpace(bytes.Length);
        bytes.CopyTo(_buffer.AsSpan(_pos));
        _pos += bytes.Length;
    }

    public void WriteInt16(short value)
    {
        EnsureSpace(2);
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += 2;
    }

    public void WriteUInt16(ushort value)
    {
        EnsureSpace(2);
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += 2;
    }

    public void WriteInt32(int value)
    {
        EnsureSpace(4);
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += 2;
    }

    public void WriteUInt32(int value)
    {
        EnsureSpace(4);
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += 2;
    }

    public void WriteSingle(float value)
    {
        EnsureSpace(sizeof(float));
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += sizeof(float);
    }

    public void WriteDouble(double value)
    {
        EnsureSpace(sizeof(double));
        BitConverter.TryWriteBytes(_buffer.AsSpan(_pos), value);
        _pos += sizeof(double);
    }

    public void WriteString(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteUInt16((ushort) bytes.Length);
        WriteBytes(bytes);
    }
}