using MirageMud.Server.Features.Characters.Entities;

namespace MirageMud.Server.Features.Characters.Commands.CreateCharacter;

internal sealed record CreateCharacterCommand(
    int AccountId,
    string CharacterName,
    CharacterSex Sex,
    int CharacterTypeId,
    int AvatarId)
    : IRequest<Option<Error>>;