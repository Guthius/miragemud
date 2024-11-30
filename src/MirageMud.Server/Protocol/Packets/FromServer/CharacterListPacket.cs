using MirageMud.Server.Dtos;
using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromServer;

public sealed record CharacterListPacket(List<CharacterSlotDto> Characters) : IPacket<CharacterListPacket>
{
    public const int Id = 2;

    public static CharacterListPacket ReadFrom(IPacketReader reader)
    {
        var characters = new List<CharacterSlotDto>();

        for (var i = 0; i < Limits.MaxCharacters; ++i)
        {
            var character = new CharacterSlotDto(
                reader.ReadInt32(),
                reader.ReadString(),
                reader.ReadString(),
                reader.ReadByte());

            characters.Add(character);
        }

        return new CharacterListPacket(characters);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(Id);

        foreach (var character in Characters)
        {
            writer.WriteInt32(character.AvatarId);
            writer.WriteString(character.CharacterName);
            writer.WriteString(character.ClassName);
            writer.WriteByte((byte) character.Level);
        }
    }
}