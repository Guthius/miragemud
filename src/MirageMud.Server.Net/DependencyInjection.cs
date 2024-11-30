using Microsoft.Extensions.DependencyInjection;

namespace MirageMud.Server.Net;

public static class DependencyInjection
{
    public static IServiceCollection AddGameService<TConnection>(this IServiceCollection services)
        where TConnection : Connection<TConnection>
    {
        services.AddTransient<TConnection>();
        services.AddHostedService<Server<TConnection>>();

        return services;
    }
}