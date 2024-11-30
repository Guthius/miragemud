using LanguageExt;
using LanguageExt.Common;
using MirageMud.Server.Domain.Entities;

namespace MirageMud.Server.Domain.Services;

public interface IAccountService
{
    Option<Error> CreateAccount(string accountName, string password);
    Either<Error, Account> Login(string accountName, string password);
}