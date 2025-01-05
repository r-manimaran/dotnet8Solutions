using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyApi.Configurations;

public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder.HasKey(up => new { up.UserId, up.ProjectId });

        builder.Property(up=>up.AssignedDate)
               .IsRequired()
               .HasDefaultValueSql("GetDate()");

        builder.Property(up => up.Role)
               .HasMaxLength(50);
    }
}
