using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CompanyApi.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Create a Index on Name in the Companies entity
        builder.HasIndex(c => c.Name)
               .IsUnique();

        // Configure one-to-many relationship with users
        builder.HasMany(c=>c.Users)
               .WithOne(u=>u.Company)
               .HasForeignKey(u=>u.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);

        // configure one-to-many relationship with Projects
        builder.HasMany(c=>c.Projects)
               .WithOne(p=>p.Company)
               .HasForeignKey(p=>p.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);        
    }
}
