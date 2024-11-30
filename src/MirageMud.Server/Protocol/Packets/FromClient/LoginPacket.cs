using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record LoginPacket(string AccountName, string Password, Version ClientVersion) : IPacket<LoginPacket>
{
    public static LoginPacket ReadFrom(IPacketReader reader)
    {
        var accountName = reader.ReadString();
        var password = reader.ReadString();
        var clientVersion = new Version(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());

        return new LoginPacket(accountName, password, clientVersion);
    }
    
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromClient.Login);
        writer.WriteString(AccountName);
        writer.WriteString(Password);
        writer.WriteByte((byte) ClientVersion.Major);
        writer.WriteByte((byte) ClientVersion.Minor);
        writer.WriteByte((byte) ClientVersion.Revision);
    }
}