using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MirageMud.Server.Features.Game.Entities.Configuration;

internal sealed class NpcConfiguration : IEntityTypeConfiguration<Npc>
{
    public void Configure(EntityTypeBuilder<Npc> builder)
    {
        builder.HasKey(npc => npc.Id);

        builder.HasData(new Npc
            {
                Id = 1,
                Name = "Bat",
                AvatarId = 2,
                SpawnSecs = 5,
                Behaviour = NpcBehaviour.AttackWhenAttacked,
                Range = 1,
                DropChance = 1,
                DropItemId = 1,
                DropItemValue = 10,
                Strength = 2,
                Defense = 2
            },
            new Npc
            {
                Id = 2,
                Name = "Bartender",
                AvatarId = 3,
                Behaviour = NpcBehaviour.Shopkeeper
            });
    }
}