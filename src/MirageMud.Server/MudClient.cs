using Microsoft.Extensions.Logging;
using MirageMud.Server.Net;
using MirageMud.Server.Protocol.Packets;
using MirageMud.Server.Protocol.Packets.FromClient;
using MirageMud.Server.Protocol.Packets.FromServer;

namespace MirageMud.Server;

internal sealed class MudClient : Connection<MudClient>
{
    public MudClientState State { get; private set; } = MudClientState.Connected;

    public MudClient(ILogger<MudClient> logger) : base(logger)
    {
        Bind<LoginPacket>(LoginPacket.Id, HandleLogin);
    }

    protected override void OnDisconnected()
    {
        State = MudClientState.Disconnected;
    }

    private void SendToAll(IPacket packet)
    {
        SendToAll(packet, client => client.State == MudClientState.InGame);
    }

    private void HandleLogin(LoginPacket packet)
    {
        Send(new AlertPacket("Login has not been implemented yet."));
    }
}