using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyApi.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(100);
            

        // Configure Many-to-One Configuration with Company
        builder.HasOne(u=>u.Company)
            .WithMany(c=>c.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-many relationshipt with Projects
        builder.HasMany(u => u.UserProjects)
               .WithOne(up => up.User)
               .HasForeignKey(up => up.UserId)
               .OnDelete(DeleteBehavior.Cascade);
             

    }
}
