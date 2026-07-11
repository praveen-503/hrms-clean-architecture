using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Designation entity.
/// </summary>
public class DesignationConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.ToTable("Designations");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        // Concurrency token
        builder.Property(d => d.RowVersion)
            .IsRowVersion();
    }
}
