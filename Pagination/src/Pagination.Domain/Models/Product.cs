namespace Pagination.Domain;

public class Product : Entity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign key
    public int CategoryId { get; set; }
}
