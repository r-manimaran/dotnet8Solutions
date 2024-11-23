namespace Pagination.Domain;

public class Category : Entity
{
    public string Name { get; set; } = null!;

    // Products Category Navigation Property
    public ICollection<Product> Products { get; set; } = new List<Product>();

}
