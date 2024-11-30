namespace MirageMud.Server.Features.Characters.Commands.DeleteCharacter;

internal sealed record DeleteCharacterCommand(int CharacterId) : IRequest;