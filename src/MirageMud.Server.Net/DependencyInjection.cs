using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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