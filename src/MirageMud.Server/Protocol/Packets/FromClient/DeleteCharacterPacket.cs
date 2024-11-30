using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record DeleteCharacterPacket(int SlotIndex) : IPacket<DeleteCharacterPacket>
{
    public static DeleteCharacterPacket ReadFrom(IPacketReader reader)
    {
        return new DeleteCharacterPacket(reader.ReadInt32() - 1);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromClient.DeleteCharacter);
        writer.WriteInt32(SlotIndex + 1);
    }
}