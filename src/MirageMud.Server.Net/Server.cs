using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MirageMud.Server.Net;

public sealed class Server<TClient, TClientState>(ILogger<Server<TClient, TClientState>> logger, IConfiguration configuration, IServiceProvider serviceProvider)
    : BackgroundService, IServer<TClient, TClientState>
    where TClient : Connection<TClient, TClientState>
    where TClientState : struct, Enum
{
    private const int DefaultPort = 7777;
    private const int DefaultMaxConnections = 100;

    private readonly int _port = configuration.GetValue("Port", DefaultPort);
    private readonly int _maxConnections = configuration.GetValue("MaxConnections", DefaultMaxConnections);
    private readonly ConcurrentQueue<int> _clientIds = [];
    private readonly ConcurrentDictionary<int, TClient> _clients = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        for (var i = 0; i < _maxConnections; i++)
        {
            _clientIds.Enqueue(i + 1);
        }

        try
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            socket.Listen((int) SocketOptionName.MaxConnections);
        }
        catch (SocketException ex)
        {
            logger.LogError(ex, "Failed to start server on port {Port}", _port);

            return;
        }

        logger.LogInformation("Server started on port {Port}", _port);

        await RunAsync(socket, stoppingToken);
    }

    private async Task RunAsync(Socket server, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var socket = await server.AcceptAsync(stoppingToken);

                await RegisterAsync(socket, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while accepting a connection");
            }
        }
    }

    private static void Close(Socket socket)
    {
        try
        {
            socket.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            socket.Close();
        }
    }

    private async Task RegisterAsync(Socket socket, CancellationToken stoppingToken)
    {
        if (!_clientIds.TryDequeue(out var clientId))
        {
            Close(socket);

            await Task.Delay(100, stoppingToken);

            return;
        }

        var client = serviceProvider.GetRequiredService<TClient>();

        _clients[clientId] = client;

        client.Run(this, clientId, socket);
    }

    internal void Unregister(int clientId)
    {
        _clients.TryRemove(clientId, out _);
        _clientIds.Enqueue(clientId);
    }

    public void SendTo(IPacket packet, Func<TClient, bool> predicate)
    {
        foreach (var client in _clients.Values)
        {
            if (!predicate(client))
            {
                continue;
            }

            client.Send(packet);
        }
    }
}