using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MirageMud.Server;
using MirageMud.Server.Domain.Services;
using MirageMud.Server.Infrastructure;
using MirageMud.Server.Infrastructure.Contexts;
using MirageMud.Server.Net;
using MirageMud.Server.Services;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(
    logger => logger
        .MinimumLevel.Verbose()
        .WriteTo.Console());

builder.Services.AddDbContext<AccountDbContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("AccountsDb")),
    ServiceLifetime.Singleton);

builder.Services.AddTransient<IAccountService, AccountService>();

builder.Services.AddGameService<MudClient>();

var app = builder.Build();

await app.EnsureDatabaseCreated<AccountDbContext>();

await app.RunAsync();