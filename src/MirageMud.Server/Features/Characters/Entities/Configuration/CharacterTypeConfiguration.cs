using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Characters.Entities.Configuration;

internal sealed class CharacterTypeConfiguration : IEntityTypeConfiguration<CharacterType>
{
    public void Configure(EntityTypeBuilder<CharacterType> builder)
    {
        builder.HasKey(characterType => characterType.Id);

        builder.HasData(new CharacterType
            {
                Id = 1,
                Name = "Mage",
                AvatarId = 2,
                Strength = 1,
                Defense = 2,
                Speed = 2,
                Magic = 5
            },
            new CharacterType
            {
                Id = 2,
                Name = "Cleric",
                AvatarId = 2,
                Strength = 2,
                Defense = 3,
                Speed = 2,
                Magic = 3
            },
            new CharacterType
            {
                Id = 3,
                Name = "Warrior",
                AvatarId = 9,
                Strength = 5,
                Defense = 3,
                Speed = 2,
                Magic = 0
            });
    }
}