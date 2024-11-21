using System.ComponentModel.DataAnnotations;

namespace EF.Pagination.Benchmark;

public class Product
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign Key
    public int CategoryId { get; set; }
    // Navigation property to Category
    public Category Category { get; set; } = null!;
}
