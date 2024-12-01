using Microsoft.Extensions.DependencyInjection;

namespace MirageMud.Server.Net;

public static class DependencyInjection
{
    public static IServiceCollection AddGameService<TClient, TClientState>(this IServiceCollection services)
        where TClient : Connection<TClient, TClientState>
        where TClientState : struct, Enum
    {
        services.AddTransient<TClient>();
        services.AddHostedService<Server<TClient, TClientState>>();

        return services;
    }
}