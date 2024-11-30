namespace MirageMud.Server.Features.Characters.Dtos;

public sealed record CharacterSlotDto(int CharacterId, int AvatarId, string CharacterName, string ClassName, int Level);