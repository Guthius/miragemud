using MirageMud.Server.Common;

namespace MirageMud.Server.Features.Game.Entities;

public sealed record RoomData
{
    public sealed record Item
    {
        public int ItemId { get; set; }
        public int Value { get; set; }
    }
    
    public sealed record ItemInventory() : Inventory<ItemInventory, Item>(Limits.MaxRoomItems);

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string ExitDescription { get; set; } = string.Empty;
    public int Revision { get; set; }
    public int Moral { get; set; }
    public int Up { get; set; }
    public int Down { get; set; }
    public int North { get; set; }
    public int East { get; set; }
    public int South { get; set; }
    public int West { get; set; }
    public string Music { get; set; } = string.Empty;
    public int BootRoomId { get; set; }
    public int ShopId { get; set; }
    public List<int> NpcIds { get; set; } = [];
    public ItemInventory Items { get; set; } = [];
}