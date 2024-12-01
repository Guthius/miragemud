using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Game.Entities.Configuration;

internal sealed class ShopConfigration : IEntityTypeConfiguration<Shop>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IncludeFields = true
    };

    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasKey(shop => shop.Id);

        builder
            .Property(shop => shop.Inventory)
            .HasConversion(
                inventory => JsonSerializer.Serialize(inventory, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<ShopInventory>(json, JsonSerializerOptions)!);
    }
}