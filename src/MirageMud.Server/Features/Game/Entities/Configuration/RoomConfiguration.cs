using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Game.Entities.Configuration;

internal sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(room => room.Id);

        builder.Property(room => room.NpcIds)
            .HasConversion(
                npcIds => JsonSerializer.Serialize(npcIds, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<int>>(json, JsonSerializerOptions.Default)!);

        builder.Property(room => room.Items)
            .HasConversion(
                items => JsonSerializer.Serialize(items, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<Room.ItemInventory>(json, JsonSerializerOptions.Default)!);

        builder.HasData(new Room
            {
                Id = 1,
                Name = "Ebersmile Tavern",
                ShortDescription = "You are in the Ebersmile Tavern, your average drinking house.",
                LongDescription = "As you look around you notice a bar maid rushing around collecting empty glasses, and a bartender stoof over at the bar serving drinks.",
                ExitDescription = "The tavern exits to the [color=#66ff00]East[/color].",
                Revision = 1,
                Moral = 2,
                East = 2,
                ShopId = 1,
                Music = "town.mp3",
                NpcIds = [2]
            },
            new Room
            {
                Id = 2,
                Name = "Orsaf Street",
                ShortDescription = "You are on a cobbled street",
                LongDescription = "The town is bustling with people, you notice buildings that line the edges of the road.",
                ExitDescription = "You can enter the Embersmile Tavern to the [color=#66ff00]West[/color].",
                Revision = 1,
                Moral = 0,
                West = 1,
                Music = "Menu.mp3",
                NpcIds = [1]
            });
    }
}