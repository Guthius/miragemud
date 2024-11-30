using MirageMud.Server.Features.Characters.Dtos;

namespace MirageMud.Server.Features.Characters.Queries.GetCharactersByAccount;

internal sealed record GetCharacterSlotsByAccountQuery(int AccountId) : IRequest<List<CharacterSlotDto>>;