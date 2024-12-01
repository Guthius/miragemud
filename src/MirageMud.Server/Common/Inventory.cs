using System.Collections;
using System.Text.Json.Serialization;

namespace MirageMud.Server.Common;

public abstract record Inventory<TSelf, TItem>(int Size) : IEnumerable<TItem>
    where TSelf : Inventory<TSelf, TItem>, new()
    where TItem : new()
{
    [JsonInclude]
    [JsonPropertyName("Slots")]
    private readonly Dictionary<int, TItem> _slots = [];

    public TItem? this[int slot] => Get(slot);

    public TItem? Get(int slot)
    {
        if (slot < 1 || slot > Size)
        {
            return default;
        }

        if (_slots.TryGetValue(slot, out var value))
        {
            return value;
        }

        value = new TItem();

        _slots[slot] = value;

        return value;
    }

    public static TSelf FromList(List<TItem> slots)
    {
        var inventory = new TSelf();

        for (var i = 0; i < slots.Count; ++i)
        {
            inventory._slots[i + 1] = slots[i];
        }

        return inventory;
    }

    public IEnumerator<TItem> GetEnumerator()
    {
        return _slots.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}