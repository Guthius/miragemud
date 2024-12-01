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
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
    }

    public DbSet<ItemData> Items => Set<ItemData>();
    public DbSet<NpcData> Npcs => Set<NpcData>();
    public DbSet<ShopData> Shops => Set<ShopData>();
    public DbSet<SpellData> Spells => Set<SpellData>();
    public DbSet<RoomData> Rooms => Set<RoomData>();
}