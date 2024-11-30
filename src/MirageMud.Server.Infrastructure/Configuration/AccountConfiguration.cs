using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MirageMud.Server.Domain.Entities;

namespace MirageMud.Server.Infrastructure.Configuration;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(account => account.Id);
        
        builder.HasIndex(account => account.Name).IsUnique();

        builder
            .Property(account => account.Password)
            .HasConversion(
                password => password.Hash,
                hash => AccountPassword.FromHash(hash));
    }
}