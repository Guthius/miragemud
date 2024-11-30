namespace MirageMud.Server.Features.Characters;

public static class Errors
{
    public static readonly Error CharacterNameTooShort =
        Error.New("Character name must be at least three characters in length.");

    public static readonly Error InvalidCharacterName =
        Error.New("Invalid name, only letters, numbers, spaces, and _ allowed in names.");

    public static readonly Error CharacterTypeDoesNotExist =
        Error.New("Character type does not exist.");

    public static readonly Error CharacterNameAlreadyExists =
        Error.New("Sorry, that character name is already taken!");
}