using MirageMud.Server.Features.Accounts.Entities;

namespace MirageMud.Server.Features.Accounts.Commands.Login;

internal sealed record LoginCommand(string AccountName, string Password) : IRequest<Either<Error, Account>>;