using MirageMud.Server.Features.Characters.Entities;

namespace MirageMud.Server.Features.Characters.Queries.GetCharacterTypes;

public sealed record GetCharacterTypesQuery : IRequest<List<CharacterType>>;