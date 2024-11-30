using Microsoft.Extensions.Hosting;
using MirageMud.Server;
using MirageMud.Server.Net;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(
    logger => logger
        .MinimumLevel.Verbose()
        .WriteTo.Console());

builder.Services.AddGameService<MudClient>();

var host = builder.Build();

await host.RunAsync();