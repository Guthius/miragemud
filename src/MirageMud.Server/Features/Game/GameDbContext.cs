using MirageMud.Server.Features.Game.Entities;
using MirageMud.Server.Features.Game.Entities.Configuration;

namespace MirageMud.Server.Features.Game;

internal sealed class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new NpcConfiguration());
        modelBuilder.ApplyConfiguration(new ShopConfigration());
        modelBuilder.ApplyConfiguration(new SpellConfiguration());
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Npc> Npcs => Set<Npc>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Spell> Spells => Set<Spell>();
}