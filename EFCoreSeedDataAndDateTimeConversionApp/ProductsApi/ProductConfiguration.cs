using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsApi.Converters;
using ProductsApi.Models;

namespace ProductsApi
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    { 
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(s => s.Name).IsRequired()
                                       .HasMaxLength(50);
            builder.Property(s => s.CreatedDate)
                .IsRequired()
                .HasConversion(new DateTimeUtcConverter());

            // To Insert Data 
            /*builder.HasData(new Product
            {
                Id = 10,
                Name = "Mouse",
                CreatedDate = DateTime.UtcNow,
            });
            builder.HasData(new Product
            {
                Id = 11,
                Name = "Keyboard",
                CreatedDate = DateTime.UtcNow,
            });*/
        }
    }
}
