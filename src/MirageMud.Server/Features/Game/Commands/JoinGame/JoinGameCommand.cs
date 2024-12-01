namespace MirageMud.Server.Features.Game.Commands.JoinGame;

internal sealed record JoinGameCommand(MudClient Client, int CharacterId) : IRequest<int>;