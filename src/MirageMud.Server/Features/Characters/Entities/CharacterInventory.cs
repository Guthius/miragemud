using System.Text.Json.Serialization;

namespace MirageMud.Server.Features.Characters.Entities;

public sealed record CharacterInventory
{
    public sealed record SlotData
    {
        public int ItemId { get; set; }
        public int Value { get; set; }
        public int Durability { get; set; }
    }

    [JsonInclude]
    [JsonPropertyName("Slots")]
    private Dictionary<int, SlotData> _slots = new();

    public SlotData? GetSlot(int slot)
    {
        if (slot is < 1 or > Limits.MaxInventorySlots)
        {
            return null;
        }

        if (_slots.TryGetValue(slot, out var slotData))
        {
            return slotData;
        }

        slotData = new SlotData();

        _slots.Add(slot, slotData);

        return slotData;
    }
}