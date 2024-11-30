namespace MirageMud.Server.Features.Characters.Commands.DeleteCharacter;

internal sealed class DeleteCharacterCommandHandler(CharacterDbContext context) : IRequestHandler<DeleteCharacterCommand>
{
    public async Task Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
    {
        await context.Characters
            .Where(character => character.Id == request.CharacterId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}