using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record CreateAccountPacket(string AccountName, string Password) : IPacket<CreateAccountPacket>
{
    public static CreateAccountPacket ReadFrom(IPacketReader reader)
    {
        var accountName = reader.ReadString();
        var password = reader.ReadString();

        return new CreateAccountPacket(accountName, password);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromClient.CreateAccount);
        writer.WriteString(AccountName);
        writer.WriteString(Password);
    }
}