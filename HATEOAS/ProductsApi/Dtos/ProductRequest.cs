namespace ProductsApi.Dtos;

public class ProductRequest
{
    public string Name { get; set; }
    public string Sku { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}
