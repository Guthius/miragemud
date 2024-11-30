using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using MirageMud.Server.Domain.Entities;
using MirageMud.Server.Domain.Services;
using MirageMud.Server.Dtos;
using MirageMud.Server.Net;
using MirageMud.Server.Protocol.Packets.FromClient;
using MirageMud.Server.Protocol.Packets.FromServer;

namespace MirageMud.Server;

internal sealed class MudClient : Connection<MudClient>
{
    private readonly IAccountService _accountService;

    public MudClientState State { get; private set; } = MudClientState.Connected;

    public MudClient(ILogger<MudClient> logger, IAccountService accountService) : base(logger)
    {
        _accountService = accountService;

        Bind<NewAccountPacket>(NewAccountPacket.Id, HandleNewAccount);
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

    private void SendError(Error error)
    {
        Send(new AlertPacket(error.Message));
    }

    private void SendCharacterList(Account account)
    {
        var emptyCharacterSlot = new CharacterSlotDto(0, string.Empty, string.Empty, 0);
        
        var characterSlots = Enumerable
            .Range(0, Limits.MaxCharacters)
            .Select((_, _) => emptyCharacterSlot)
            .ToList();

        Send(new CharacterListPacket(characterSlots));
    }

    private void HandleNewAccount(NewAccountPacket packet)
    {
        _accountService
            .CreateAccount(packet.AccountName, packet.Password)
            .Match(SendError, SendAccountCreated);

        return;

        void SendAccountCreated()
        {
            Send(new AlertPacket("Your account has been created!"));
        }
    }

    private void HandleLogin(LoginPacket packet)
    {
        _accountService
            .Login(packet.AccountName, packet.Password)
            .Match(SendCharacterList, SendError);
    }
}