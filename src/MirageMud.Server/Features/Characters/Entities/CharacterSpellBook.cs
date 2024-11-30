using System.Text.Json.Serialization;

namespace MirageMud.Server.Features.Characters.Entities;

public sealed record CharacterSpellBook
{
    [JsonInclude]
    [JsonPropertyName("Slots")]
    private Dictionary<int, int> _slots = new();

    public int GetSpellId(int slot)
    {
        if (slot is < 1 or > Limits.MaxCharacterSpells)
        {
            return 0;
        }

        if (_slots.TryGetValue(slot, out var spellId))
        {
            return spellId;
        }

        _slots.Add(slot, 0);

        return 0;
    }

    public void SetSpellId(int slot, int spellId)
    {
        if (slot is < 1 or > Limits.MaxCharacterSpells)
        {
            return;
        }

        _slots[slot] = spellId;
    }
}