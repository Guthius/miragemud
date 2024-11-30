using MirageMud.Server.Features.Characters.Entities;
using MirageMud.Server.Features.Characters.Entities.Configuration;

namespace MirageMud.Server.Features.Characters;

public sealed class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CharacterConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterTypeConfiguration());
    }

    public DbSet<Character> Characters => Set<Character>();
    public DbSet<CharacterType> CharacterTypes => Set<CharacterType>();
}