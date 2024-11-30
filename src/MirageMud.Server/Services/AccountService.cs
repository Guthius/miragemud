using LanguageExt;
using LanguageExt.Common;
using MirageMud.Server.Domain.Entities;
using MirageMud.Server.Domain.Services;
using MirageMud.Server.Infrastructure.Contexts;

namespace MirageMud.Server.Services;

internal sealed class AccountService(AccountDbContext context) : IAccountService
{
    private static class Errors
    {
        public static readonly Error AccountNameOrPasswordTooShort =
            Error.New("Your name and password must be at least three characters in length");

        public static readonly Error InvalidAccountName =
            Error.New("Invalid name, only letters, numbers, spaces, and _ allowed in names.");

        public static readonly Error AccountAlreadyExists =
            Error.New("Sorry, that account name is already taken!");

        public static readonly Error AccountDoesNotExist =
            Error.New("That account name does not exist.");

        public static readonly Error InvalidPassword =
            Error.New("Incorrect password.");
    }

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

    public Option<Error> CreateAccount(string accountName, string password)
    {
        if (accountName.Length < 3 || password.Length < 3)
        {
            return Errors.AccountNameOrPasswordTooShort;
        }

        if (!IsValidAccountName(accountName))
        {
            return Errors.InvalidAccountName;
        }

        var account = context.Accounts.FirstOrDefault(account => account.Name == accountName);
        if (account is not null)
        {
            return Errors.AccountAlreadyExists;
        }

        account = new Account
        {
            Name = accountName,
            Password = AccountPassword.FromPlainText(password)
        };

        context.Accounts.Add(account);
        context.SaveChanges();

        return Option<Error>.None;
    }

    public Either<Error, Account> Login(string accountName, string password)
    {
        if (accountName.Length < 3 || password.Length < 3)
        {
            return Errors.AccountNameOrPasswordTooShort;
        }

        var account = context.Accounts.FirstOrDefault(account => account.Name == accountName);
        if (account is null)
        {
            return Errors.AccountDoesNotExist;
        }

        if (!account.Password.Verify(password))
        {
            return Errors.InvalidPassword;
        }

        return account;
    }
}