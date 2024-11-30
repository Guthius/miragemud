using MirageMud.Server.Features.Characters.Entities;
using MirageMud.Server.Net;

namespace MirageMud.Server.Protocol.Packets.FromServer;

public sealed record CharacterTypesPacket(IReadOnlyList<CharacterType> Types) : IPacket<CharacterTypesPacket>
{
    public static CharacterTypesPacket ReadFrom(IPacketReader reader)
    {
        var count = reader.ReadByte();
        
        var types = new List<CharacterType>();
        
        for (var i = 0; i < count; ++i)
        {
            var name = reader.ReadString();
            var avatarId = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            var strength = reader.ReadByte();
            var defense = reader.ReadByte();
            var speed = reader.ReadByte();
            var magic = reader.ReadByte();
            
            types.Add(new CharacterType
            {
                Id = i + 1,
                Name = name,
                AvatarId = avatarId,
                Strength = strength,
                Defense = defense,
                Speed = speed,
                Magic = magic
            });
        }

        return new CharacterTypesPacket(types);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt16(PacketId.FromServer.CharacterTypes);
        writer.WriteByte((byte) Types.Count);

        foreach (var type in Types)
        {
            writer.WriteString(type.Name);
            writer.WriteInt32(type.AvatarId);

            writer.WriteInt32(type.MaxHP);
            writer.WriteInt32(type.MaxMP);
            writer.WriteInt32(type.MaxSP);
            writer.WriteInt32(0); // TODO: MaxStamina is not calculated by the VB6 server, can be dropped?

            writer.WriteByte((byte) type.Strength);
            writer.WriteByte((byte) type.Defense);
            writer.WriteByte((byte) type.Speed);
            writer.WriteByte((byte) type.Magic);
        }
    }
}