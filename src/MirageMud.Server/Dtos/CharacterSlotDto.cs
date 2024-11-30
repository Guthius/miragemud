namespace MirageMud.Server.Dtos;

public sealed record CharacterSlotDto(int AvatarId, string CharacterName, string ClassName, int Level);