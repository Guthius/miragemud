using MirageMud.Server.Features.Characters.Entities;

namespace MirageMud.Server.Features.Characters.Queries.GetCharacterTypes;

internal sealed class GetCharacterTypesQueryHandler(CharacterDbContext context) : IRequestHandler<GetCharacterTypesQuery, List<CharacterType>>
{
    public async Task<List<CharacterType>> Handle(GetCharacterTypesQuery query, CancellationToken cancellationToken)
    {
        return await context.CharacterTypes
            .AsNoTracking()
            .OrderBy(characterType => characterType.Id)
            .ToListAsync(cancellationToken);
    }
}