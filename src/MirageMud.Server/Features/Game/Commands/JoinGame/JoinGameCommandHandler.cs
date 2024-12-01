namespace MirageMud.Server.Features.Game.Commands.JoinGame;

internal sealed record JoinGameCommandHandler : IRequestHandler<JoinGameCommand, int>
{
    public Task<int> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}