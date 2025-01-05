using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyApi.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p=>p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Description)
               .HasMaxLength(200);

        // Configure many to one relation with Company
        builder.HasOne(p => p.Company)
               .WithMany(c => c.Projects)
               .HasForeignKey(p => p.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-many relationship with Users
        builder.HasMany(p => p.UserProjects)
               .WithOne(up => up.Project)
               .HasForeignKey(up => up.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
