using MirageMud.Server.Features.Accounts.Entities;

namespace MirageMud.Server.Features.Accounts.Commands.Login;

internal sealed class LoginCommandHandler(AccountDbContext context) : IRequestHandler<LoginCommand, Either<Error, Account>>
{
    public async Task<Either<Error, Account>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        if (command.AccountName.Length < 3 || command.Password.Length < 3)
        {
            return Errors.AccountNameOrPasswordTooShort;
        }

        var account = await context.Accounts.FirstOrDefaultAsync(account => account.Name == command.AccountName, cancellationToken: cancellationToken);
        if (account is null)
        {
            return Errors.AccountDoesNotExist;
        }

        if (!account.Password.Verify(command.Password))
        {
            return Errors.InvalidPassword;
        }

        return account;
    }
}