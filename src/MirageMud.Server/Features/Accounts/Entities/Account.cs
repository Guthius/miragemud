﻿namespace MirageMud.Server.Features.Accounts.Entities;

public sealed record Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AccountPassword Password { get; set; } = AccountPassword.Empty;
}