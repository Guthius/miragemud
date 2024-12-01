using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Game.Entities.Configuration;

internal sealed class SpellConfiguration : IEntityTypeConfiguration<SpellData>
{
    public void Configure(EntityTypeBuilder<SpellData> builder)
    {
        builder.HasKey(spell => spell.Id);
    }
}