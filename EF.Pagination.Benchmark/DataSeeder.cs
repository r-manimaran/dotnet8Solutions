namespace EF.Pagination.Benchmark;
using Bogus;

public class DataSeeder
{
    public static void SeedData(AppDbContext context)
    {
        // Category Faker
        var categoyFaker = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);

        // product Faker
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
            .RuleFor(p=>p.Stock, f=>f.Random.Int(1,1000))
            .RuleFor(p=>p.CreatedAt, f=>f.Date.Past(10))
            .RuleFor(p => p.Category, f => categoyFaker.Generate());
        
        // Generate 50 categories
        var categories = categoyFaker.Generate(50);
        context.Categories.AddRange(categories);
        Console.WriteLine($"Generated {categories.Count} categories and inserted");
        //Generate 10000 products in total, 200 products in each category
        foreach (var category in categories)
        {
            var products = productFaker.RuleFor(p => p.Category, category).Generate(200);
            context.Products.AddRange(products);
            Console.WriteLine($"Category {category.Name} has {products.Count} products");
        }

        context.SaveChanges();
        Console.WriteLine("Data seeded successfully");
        
    }
}
