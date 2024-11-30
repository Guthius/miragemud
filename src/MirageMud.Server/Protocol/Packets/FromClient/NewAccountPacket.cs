using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record NewAccountPacket(string AccountName, string Password) : IPacket<NewAccountPacket>
{
    public const int Id = 2;

    public static NewAccountPacket ReadFrom(IPacketReader reader)
    {
        var accountName = reader.ReadString();
        var password = reader.ReadString();

        return new NewAccountPacket(accountName, password);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(Id);
        writer.WriteString(AccountName);
        writer.WriteString(Password);
    }
}