using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Employee entity.
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Salary)
            .HasPrecision(18, 2);

        // Owned Value Object configuration
        builder.OwnsOne(e => e.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("Address_Street");

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Address_City");

            address.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Address_State");

            address.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Address_Country");

            address.Property(a => a.ZipCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("Address_ZipCode");
        });

        // Relationships
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Designation)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DesignationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Concurrency token
        builder.Property(e => e.RowVersion)
            .IsRowVersion();
    }
}
