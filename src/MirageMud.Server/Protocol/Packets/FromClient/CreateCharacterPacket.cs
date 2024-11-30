using MirageMud.Server.Features.Characters.Entities;
using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromClient;

public sealed record CreateCharacterPacket(string CharacterName, CharacterSex Sex, int CharacterTypeId, int SlotIndex, int AvatarId) : IPacket<CreateCharacterPacket>
{
    public static CreateCharacterPacket ReadFrom(IPacketReader reader)
    {
        return new CreateCharacterPacket(
            reader.ReadString(),
            (CharacterSex) reader.ReadInt32(),
            reader.ReadInt32(),
            reader.ReadInt32() - 1,
            reader.ReadInt32());
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromClient.CreateCharacter);
        writer.WriteString(CharacterName);
        writer.WriteInt32((int) Sex);
        writer.WriteInt32(CharacterTypeId);
        writer.WriteInt32(SlotIndex + 1);
        writer.WriteInt32(AvatarId);
    }
}