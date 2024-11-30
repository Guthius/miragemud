namespace MirageMud.Server.Features.Accounts.Entities;

public sealed record AccountPassword(string Hash)
{
    public static readonly AccountPassword Empty = new(string.Empty);
    
    public bool Verify(string hash)
    {
        return BCrypt.Net.BCrypt.Verify(hash, Hash);
    }

    public static AccountPassword FromHash(string hash)
    {
        return new AccountPassword(hash);
    }

    public static AccountPassword FromPlainText(string password)
    {
        return FromHash(BCrypt.Net.BCrypt.HashPassword(password));
    }
}