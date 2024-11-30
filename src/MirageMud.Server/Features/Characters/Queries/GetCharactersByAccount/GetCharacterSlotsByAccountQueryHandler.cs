using MirageMud.Server.Features.Characters.Dtos;

namespace MirageMud.Server.Features.Characters.Queries.GetCharactersByAccount;

internal sealed class GetCharacterSlotsByAccountQueryHandler(CharacterDbContext context) : IRequestHandler<GetCharacterSlotsByAccountQuery, List<CharacterSlotDto>>
{
    public async Task<List<CharacterSlotDto>> Handle(GetCharacterSlotsByAccountQuery query, CancellationToken cancellationToken)
    {
        return await context.Characters
            .AsNoTracking()
            .Where(character =>
                character.AccountId == query.AccountId)
            .Join(context.CharacterTypes, c => c.CharacterTypeId, ct => ct.Id,
                (character, characterType) => new CharacterSlotDto(
                    character.Id,
                    character.AvatarId,
                    character.Name,
                    characterType.Name,
                    character.Level))
            .ToListAsync(cancellationToken);
    }
}