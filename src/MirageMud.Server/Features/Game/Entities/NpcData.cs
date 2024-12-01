namespace MirageMud.Server.Features.Game.Entities;

public sealed record NpcData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AttackSay { get; set; } = string.Empty;
    public int AvatarId { get; set; }
    public int SpawnSecs { get; set; }
    public NpcBehaviour Behaviour { get; set; }
    public int Range { get; set; }
    public int DropChance { get; set; }
    public int DropItemId { get; set; }
    public int DropItemValue { get; set; }
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Magic { get; set; }
}