using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MirageMud.Server.Infrastructure;

public static class DependencyInjection
{
    public static async Task EnsureDatabaseCreated<TContext>(this IHost app) where TContext : DbContext
    {
        using var serviceScope = app.Services.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.EnsureCreatedAsync();
    }
}