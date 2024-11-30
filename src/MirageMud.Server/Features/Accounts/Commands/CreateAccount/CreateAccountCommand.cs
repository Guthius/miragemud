namespace MirageMud.Server.Features.Accounts.Commands.CreateAccount;

internal sealed record CreateAccountCommand(string AccountName, string Password) : IRequest<Option<Error>>;