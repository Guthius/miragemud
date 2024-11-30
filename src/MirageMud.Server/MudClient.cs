using Microsoft.Extensions.Logging;
using MirageMud.Server.Features.Accounts.Commands.CreateAccount;
using MirageMud.Server.Features.Accounts.Commands.Login;
using MirageMud.Server.Features.Accounts.Entities;
using MirageMud.Server.Features.Characters.Commands.CreateCharacter;
using MirageMud.Server.Features.Characters.Commands.DeleteCharacter;
using MirageMud.Server.Features.Characters.Dtos;
using MirageMud.Server.Features.Characters.Queries.GetCharactersByAccount;
using MirageMud.Server.Features.Characters.Queries.GetCharacterTypes;
using MirageMud.Server.Net;
using MirageMud.Server.Protocol;
using MirageMud.Server.Protocol.Packets.FromClient;
using MirageMud.Server.Protocol.Packets.FromServer;

namespace MirageMud.Server;

internal sealed class MudClient : Connection<MudClient, MudClientState>
{
    private readonly IMediator _mediator;
    private readonly List<CharacterSlotDto> _characterSlots = [];
    private Account? _account;

    public MudClient(ILogger<MudClient> logger, IMediator mediator) : base(logger)
    {
        _mediator = mediator;

        When(MudClientState.Connected, state =>
        {
            state.Bind<CreateAccountPacket>(PacketId.FromClient.CreateAccount, HandleCreateAccount);
            state.Bind<LoginPacket>(PacketId.FromClient.Login, HandleLogin);
        });

        When(MudClientState.Authenticated, state =>
        {
            state.Bind(PacketId.FromClient.GetCharacterTypes, HandleGetCharacterTypes);
            state.Bind<CreateCharacterPacket>(PacketId.FromClient.CreateCharacter, HandleCreateCharacter);
            state.Bind<DeleteCharacterPacket>(PacketId.FromClient.DeleteCharacter, HandleDeleteCharacter);
        });
    }

    protected override void OnDisconnected()
    {
        SetState(MudClientState.Disconnected);
    }

    private void SendAlert(Error error)
    {
        Send(new AlertPacket(error.Message));

        SetState(MudClientState.Connected);
    }

    private async Task HandleGetCharacterTypes()
    {
        var types = await _mediator.Send(new GetCharacterTypesQuery());

        Send(new CharacterTypesPacket(types));
    }

    private async Task HandleCreateAccount(CreateAccountPacket packet)
    {
        await _mediator
            .Send(new CreateAccountCommand(packet.AccountName, packet.Password))
            .Match(SendAlert, SendAccountCreated);

        return;

        void SendAccountCreated()
        {
            SendAlert("Your account has been created!");
        }
    }

    private async Task HandleLogin(LoginPacket packet)
    {
        var result = await _mediator
            .Send(new LoginCommand(packet.AccountName, packet.Password))
            .MapAsync(LoadAccountDetails);

        result.Match(SendCharacterList, SendAlert);

        return;

        async Task<List<CharacterSlotDto>> LoadAccountDetails(Account account)
        {
            _account = account;

            SetState(MudClientState.Authenticated);

            var characters = await _mediator.Send(new GetCharacterSlotsByAccountQuery(account.Id));

            _characterSlots.Clear();
            _characterSlots.AddRange(characters);

            return characters;
        }

        void SendCharacterList(List<CharacterSlotDto> characterSlots)
        {
            var characterSlotsToSend = characterSlots.Take(Limits.MaxCharacters).ToList();

            while (characterSlotsToSend.Count < Limits.MaxCharacters)
            {
                characterSlotsToSend.Add(new CharacterSlotDto(0, 0, string.Empty, string.Empty, 0));
            }

            Send(new CharacterListPacket(characterSlotsToSend));
        }
    }

    private async Task HandleCreateCharacter(CreateCharacterPacket packet)
    {
        if (_account is null)
        {
            SendAlert("Not authenticated");
            return;
        }

        if (packet.SlotIndex is < 0 or >= Limits.MaxCharacters)
        {
            SendAlert("Invalid character slot");
            return;
        }

        if (packet.SlotIndex < _characterSlots.Count)
        {
            SendAlert("Character slot already in use");
            return;
        }

        var result = await _mediator.Send(new CreateCharacterCommand(
            _account.Id,
            packet.CharacterName,
            packet.Sex,
            packet.CharacterTypeId,
            packet.AvatarId));

        result.Match(SendAlert, SendCharacterCreated);

        return;

        void SendCharacterCreated()
        {
            SendAlert("Character has been created!");
        }
    }

    private async Task HandleDeleteCharacter(DeleteCharacterPacket packet)
    {
        if (packet.SlotIndex < 0 || packet.SlotIndex >= _characterSlots.Count)
        {
            SendAlert("Invalid character slot");
            return;
        }

        await _mediator.Send(new DeleteCharacterCommand(_characterSlots[packet.SlotIndex].CharacterId));

        SendAlert("Character has been deleted!");
    }
}