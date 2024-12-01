using MirageMud.Server.Common;

namespace MirageMud.Server.Features.Game.Entities;

public sealed record ShopInventory() : Inventory<ShopInventory, ShopInventory.Slot>(Limits.MaxTrades)
{
    public sealed record Slot
    {
        public int GiveItemId { get; set; }
        public int GiveQuantity { get; set; }
        public int GetItemId { get; set; }
        public int GetQuantity { get; set; }
    }
}