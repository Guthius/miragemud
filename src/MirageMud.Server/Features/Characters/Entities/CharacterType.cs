namespace MirageMud.Server.Features.Characters.Entities;

public sealed record CharacterType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AvatarId { get; set; }
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Magic { get; set; }
    public int MaxHP => (1 + Strength / 2 + Strength) * 2;
    public int MaxMP => (1 + Magic / 2 + Magic) * 2;
    public int MaxSP => (1 + Speed / 2 + Speed) * 2;
}
