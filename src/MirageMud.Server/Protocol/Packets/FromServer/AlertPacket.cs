using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromServer;

public sealed record AlertPacket(string Message) : IPacket<AlertPacket>
{
    public const int Id = 1;

    public static AlertPacket ReadFrom(IPacketReader reader)
    {
        return new AlertPacket(reader.ReadString());
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(Id);
        writer.WriteString(Message);
    }
}