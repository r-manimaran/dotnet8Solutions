using System;

namespace pagination.WebApi.Dtos;

public class ProductResultDto
{
    public string Name { get; init; }

    public decimal Price { get; init; }
    
    public DateTime CreatedAt { get; init; }

}
