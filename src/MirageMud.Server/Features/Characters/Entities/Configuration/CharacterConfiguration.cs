using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Characters.Entities.Configuration;

internal sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IncludeFields = true
    };
    
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(character => character.Id);
        builder.HasIndex(character => character.Name).IsUnique();

        builder
            .Property(character => character.Inventory)
            .HasConversion(
                inventory => JsonSerializer.Serialize(inventory, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<CharacterInventory>(json, JsonSerializerOptions)!);

        builder
            .Property(character => character.Spells)
            .HasConversion(
                inventory => JsonSerializer.Serialize(inventory, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<CharacterSpellBook>(json, JsonSerializerOptions)!);
    }
}