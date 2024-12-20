namespace EFCoreRelations.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    // One Category contains Multiple Products
    // Navigation Property
    public ICollection<Product> Products { get; set; }
}
