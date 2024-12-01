namespace MirageMud.Server.Features.Game.Entities;

public sealed record ItemData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sprite { get; set; }
    public ItemType Type { get; set; } = ItemType.None;
    public int Data1 { get; set; }
    public int Data2 { get; set; }
    public int Data3 { get; set; }
}