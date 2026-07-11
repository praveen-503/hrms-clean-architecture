using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Department entity.
/// </summary>
public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(d => d.Code)
            .IsUnique();

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        // Concurrency token
        builder.Property(d => d.RowVersion)
            .IsRowVersion();
    }
}
