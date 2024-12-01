using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record SelectCharacterPacket(int SlotIndex) : IPacket<SelectCharacterPacket>
{
    public static SelectCharacterPacket ReadFrom(IPacketReader reader)
    {
        return new SelectCharacterPacket(reader.ReadInt32() - 1);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromClient.SelectCharacter);
        writer.WriteInt32(SlotIndex + 1);
    }
}