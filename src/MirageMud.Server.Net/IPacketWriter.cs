namespace MirageMud.Server.Net;

public interface IPacketWriter
{
    void WriteByte(byte value);
    void WriteBytes(ReadOnlySpan<byte> bytes);
    void WriteInt16(short value);
    void WriteUInt16(ushort value);
    void WriteInt32(int value);
    void WriteUInt32(int value);
    void WriteSingle(float value);
    void WriteDouble(double value);
    void WriteString(string value);
}