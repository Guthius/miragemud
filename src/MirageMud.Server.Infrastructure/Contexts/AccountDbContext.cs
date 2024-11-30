using Microsoft.EntityFrameworkCore;
using MirageMud.Server.Domain.Entities;

namespace MirageMud.Server.Infrastructure.Contexts;

public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);
    }

    public DbSet<Account> Accounts => Set<Account>();
}