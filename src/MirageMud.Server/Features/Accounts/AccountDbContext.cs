using MirageMud.Server.Features.Accounts.Entities;
using MirageMud.Server.Features.Accounts.Entities.Configuration;

namespace MirageMud.Server.Features.Accounts;

internal sealed class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }

    public DbSet<Account> Accounts => Set<Account>();
}