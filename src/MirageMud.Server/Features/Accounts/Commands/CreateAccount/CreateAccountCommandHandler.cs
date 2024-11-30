using System.Diagnostics.Contracts;
using MirageMud.Server.Features.Accounts.Entities;

namespace MirageMud.Server.Features.Accounts.Commands.CreateAccount;

internal sealed class CreateAccountCommandHandler(AccountDbContext context) : IRequestHandler<CreateAccountCommand, Option<Error>>
{
    public async Task<Option<Error>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        if (command.AccountName is not {Length: > 3} ||
            command.Password is not {Length: > 3})
        {
            return Errors.AccountNameOrPasswordTooShort;
        }

        if (!IsValidAccountName(command.AccountName))
        {
            return Errors.InvalidAccountName;
        }

        var account = await context.Accounts.FirstOrDefaultAsync(account => account.Name == command.AccountName, cancellationToken);
        if (account is not null)
        {
            return Errors.AccountAlreadyExists;
        }

        account = new Account
        {
            Name = command.AccountName,
            Password = AccountPassword.FromPlainText(command.Password)
        };

        context.Accounts.Add(account);

        await context.SaveChangesAsync(cancellationToken);

        return Option<Error>.None;
    }

    [Pure]
    private static bool IsValidAccountName(ReadOnlySpan<char> accountName)
    {
        foreach (var ch in accountName)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != ' ')
            {
                return false;
            }
        }

        return true;
    }
}