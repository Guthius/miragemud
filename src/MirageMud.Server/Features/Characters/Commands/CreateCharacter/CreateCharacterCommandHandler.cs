using System.Diagnostics.Contracts;
using MirageMud.Server.Features.Characters.Entities;

namespace MirageMud.Server.Features.Characters.Commands.CreateCharacter;

internal sealed class CreateCharacterCommandHandler(CharacterDbContext context) : IRequestHandler<CreateCharacterCommand, Option<Error>>
{
    public async Task<Option<Error>> Handle(CreateCharacterCommand command, CancellationToken cancellationToken)
    {
        if (command.CharacterName is not {Length: > 3})
        {
            return Errors.CharacterNameTooShort;
        }

        if (!IsValidCharacterName(command.CharacterName))
        {
            return Errors.InvalidCharacterName;
        }

        var characterType = await context.CharacterTypes.FindAsync([command.CharacterTypeId], cancellationToken: cancellationToken);
        if (characterType is null)
        {
            return Errors.CharacterTypeDoesNotExist;
        }

        var nameTaken = await context.Characters.AnyAsync(x => x.Name == command.CharacterName, cancellationToken);
        if (nameTaken)
        {
            return Errors.CharacterNameAlreadyExists;
        }

        var character = new Character
        {
            AccountId = command.AccountId,
            Name = command.CharacterName,
            Sex = command.Sex,
            CharacterTypeId = characterType.Id,
            CharacterType = characterType,
            AvatarId = command.AvatarId,
            Strength = characterType.Strength,
            Defense = characterType.Defense,
            Speed = characterType.Speed,
            Magic = characterType.Magic
        };

        character.HP = character.MaxHP;
        character.MP = character.MaxMP;
        character.SP = character.MaxSP;
        character.Stamina = character.MaxStamina;

        context.Characters.Add(character);

        await context.SaveChangesAsync(cancellationToken);

        return Option<Error>.None;
    }

    [Pure]
    private static bool IsValidCharacterName(ReadOnlySpan<char> characterName)
    {
        foreach (var ch in characterName)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != ' ')
            {
                return false;
            }
        }

        return true;
    }
}