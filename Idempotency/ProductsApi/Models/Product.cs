namespace ProductsApi.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}
