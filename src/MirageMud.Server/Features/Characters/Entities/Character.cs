namespace MirageMud.Server.Features.Characters.Entities;

public sealed record Character
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CharacterSex Sex { get; set; } = CharacterSex.Male;
    public int CharacterTypeId { get; set; }
    public CharacterType CharacterType { get; set; } = null!;
    public int AvatarId { get; set; }
    public int Level { get; set; } = 1;
    public CharacterAccessLevel AccessLevel { get; set; } = CharacterAccessLevel.Player;
    public bool Pker { get; set; }
    public string Guild { get; set; } = string.Empty;
    public int GuildAccess { get; set; }

    public int HP { get; set; }
    public int MaxHP => (Level + Strength / 2 + CharacterType.Strength) * 2;
    public int MP { get; set; }
    public int MaxMP => (Level + Magic / 2 + CharacterType.Magic) * 2;
    public int SP { get; set; }
    public int MaxSP => (Level + Speed / 2 + CharacterType.Speed) * 2;
    public int Stamina { get; set; }
    public int MaxStamina
    {
        get
        {
            return Level switch
            {
                >= 1 and <= 3 => 1,
                >= 4 and <= 9 => 2,
                >= 10 and <= 14 => 3,
                >= 15 and <= 21 => 4,
                >= 22 and <= 27 => 5,
                _ => 6
            };
        }
    }

    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Magic { get; set; }
    public int Points { get; set; }

    public int Weapon { get; set; }
    public int Armor { get; set; }
    public int Helmet { get; set; }
    public int Shield { get; set; }

    public CharacterInventory Inventory { get; set; } = new();
    public CharacterSpellBook Spells { get; set; } = new();
    public int RoomId { get; set; }
    public CharacterDirection Direction { get; set; }
}