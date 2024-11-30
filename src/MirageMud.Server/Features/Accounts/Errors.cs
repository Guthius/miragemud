namespace MirageMud.Server.Features.Accounts;

internal static class Errors
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