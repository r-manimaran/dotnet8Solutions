﻿namespace ProductsApi.Models;

public class IdempotentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}
