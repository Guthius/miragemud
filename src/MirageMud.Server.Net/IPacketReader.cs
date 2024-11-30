namespace MirageMud.Server.Net;

public interface IPacketReader
{
    byte ReadByte();
    byte[] ReadBytes(int count);
    short ReadInt16();
    ushort ReadUInt16();
    int ReadInt32();
    uint ReadUInt32();
    float ReadSingle();
    double ReadDouble();
    string ReadString();
}