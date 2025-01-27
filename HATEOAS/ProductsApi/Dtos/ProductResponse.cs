namespace ProductsApi.Dtos;

public record ProductResponse (Guid Id, 
                        string Name, 
                        string Sku, 
                        string Currency, 
                        decimal Amount)
{
    public List<Link> Links { get; set; } = new List<Link> ();
}

