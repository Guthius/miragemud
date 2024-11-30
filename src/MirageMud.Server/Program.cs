using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MirageMud.Server;
using MirageMud.Server.Extensions;
using MirageMud.Server.Features.Accounts;
using MirageMud.Server.Features.Characters;
using MirageMud.Server.Net;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(
    logger => logger
        .MinimumLevel.Information()
        .WriteTo.Console());

builder.Services.AddDbContext<AccountDbContext>(
    options => options
        .UseSqlite(
            builder.Configuration.GetConnectionString("AccountsDb"))
        .UseSnakeCaseNamingConvention(),
    ServiceLifetime.Singleton);

builder.Services.AddDbContext<CharacterDbContext>(
    options => options
        .UseSqlite(
            builder.Configuration.GetConnectionString("CharactersDb"))
        .UseSnakeCaseNamingConvention(),
    ServiceLifetime.Singleton);

builder.Services.AddMemoryCache();
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddGameService<MudClient, MudClientState>();

var app = builder.Build();

await app.EnsureDatabaseCreated<AccountDbContext>();
await app.EnsureDatabaseCreated<CharacterDbContext>();

await app.RunAsync();