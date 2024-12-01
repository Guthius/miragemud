namespace MirageMud.Server.Features.Game.Entities;

public sealed record SpellData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sprite { get; set; }
    public int RequiredMp { get; set; }
    public int RequiredCharacterTypeId { get; set; }
    public int RequiredLevel { get; set; }
    public SpellType Type { get; set; } = SpellType.SubHp;
    public int Data1 { get; set; }
    public int Data2 { get; set; }
    public int Data3 { get; set; }
}