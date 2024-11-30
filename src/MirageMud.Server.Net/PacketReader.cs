using System.Text;

namespace MirageMud.Server.Net;

internal sealed class PacketReader(ReadOnlySpan<byte> bytes) : IPacketReader
{
    private readonly byte[] _bytes = bytes.ToArray();
    private int _pos;

    public byte ReadByte()
    {
        return _bytes[_pos++];
    }

    public byte[] ReadBytes(int count)
    {
        var bytes = _bytes.AsSpan(_pos, count).ToArray();
        _pos += count;
        return bytes;
    }

    public short ReadInt16()
    {
        var value = BitConverter.ToInt16(_bytes.AsSpan(_pos));
        _pos += 2;
        return value;
    }

    public ushort ReadUInt16()
    {
        var value = BitConverter.ToUInt16(_bytes.AsSpan(_pos));
        _pos += 2;
        return value;
    }

    public int ReadInt32()
    {
        var value = BitConverter.ToInt32(_bytes.AsSpan(_pos));
        _pos += 4;
        return value;
    }

    public uint ReadUInt32()
    {
        var value = BitConverter.ToUInt32(_bytes.AsSpan(_pos));
        _pos += 4;
        return value;
    }

    public float ReadSingle()
    {
        var value = BitConverter.ToSingle(_bytes.AsSpan(_pos));
        _pos += 4;
        return value;
    }

    public double ReadDouble()
    {
        var value = BitConverter.ToDouble(_bytes.AsSpan(_pos));
        _pos += 4;
        return value;
    }

    public string ReadString()
    {
        var bytes = ReadBytes(ReadInt16());
        return Encoding.UTF8.GetString(bytes);
    }
}