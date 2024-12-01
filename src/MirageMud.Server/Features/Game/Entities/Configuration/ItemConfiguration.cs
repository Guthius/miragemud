using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Game.Entities.Configuration;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<ItemData>
{
    public void Configure(EntityTypeBuilder<ItemData> builder)
    {
        builder.HasKey(item => item.Id);

        builder.HasData(new ItemData
            {
                Id = 1,
                Name = "Gold",
                Sprite = 8,
                Type = ItemType.Currency
            },
            new ItemData
            {
                Id = 2,
                Name = "Sword",
                Sprite = 9,
                Type = ItemType.Weapon,
                Data1 = 50,
                Data2 = 2,
            });
    }
}