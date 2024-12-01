namespace MirageMud.Server.Features.Game.Entities;

public sealed record ShopData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string JoinSay { get; set; } = string.Empty;
    public string LeaveSay { get; set; } = string.Empty;
    public bool FixesItems { get; set; }
    public ShopInventory Inventory { get; set; } = new();
}