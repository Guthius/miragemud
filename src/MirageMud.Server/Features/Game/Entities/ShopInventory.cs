using System.Text.Json.Serialization;

namespace MirageMud.Server.Features.Game.Entities;

public sealed record ShopInventory
{
    public sealed record Slot
    {
        public int GiveItemId { get; set; }
        public int GiveValue { get; set; }
        public int GetItemId { get; set; }
        public int GetValue { get; set; }
    }

    [JsonInclude]
    [JsonPropertyName("Slots")]
    private readonly Dictionary<int, Slot> _slots = [];

    public Slot? GetSlot(int slotId)
    {
        if (slotId is < 1 or > Limits.MaxTrades)
        {
            return null;
        }

        if (_slots.TryGetValue(slotId, out var slot))
        {
            return slot;
        }

        slot = new Slot();

        _slots[slotId] = slot;

        return slot;
    }
}