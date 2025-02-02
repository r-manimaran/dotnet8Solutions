namespace ProductsApi.DTOs;

public record CreateProductRequest(string Name, string Sku, string Currency, decimal Amount);

