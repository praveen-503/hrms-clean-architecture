using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Persistence.Seed;

/// <summary>
/// Handles database migration execution and initial data seeding.
/// </summary>
public static class HRPayrollDbContextSeed
{
    public static async Task SeedAsync(HRPayrollDbContext context)
    {
        // Apply migrations automatically
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }

        // Seed Departments
        if (!await context.Set<Department>().AnyAsync())
        {
            var departments = new[]
            {
                new Department 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Human Resources", 
                    Code = "HR", 
                    Description = "Manages employee relations, onboarding, and benefits." 
                },
                new Department 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Finance & Payroll", 
                    Code = "FIN", 
                    Description = "Handles organizational budgets, tax planning, and payroll processing." 
                },
                new Department 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Engineering", 
                    Code = "ENG", 
                    Description = "Core software development, architecture, and systems engineering." 
                }
            };

            await context.Set<Department>().AddRangeAsync(departments);
            await context.SaveChangesAsync();
        }

        // Seed Designations
        if (!await context.Set<Designation>().AnyAsync())
        {
            var designations = new[]
            {
                new Designation 
                { 
                    Id = Guid.NewGuid(), 
                    Title = "HR Admin", 
                    Description = "Human Resources Administrator" 
                },
                new Designation 
                { 
                    Id = Guid.NewGuid(), 
                    Title = "Payroll Manager", 
                    Description = "Manages employee salary structures and benefits payouts" 
                },
                new Designation 
                { 
                    Id = Guid.NewGuid(), 
                    Title = "Senior Software Engineer", 
                    Description = "Enterprise full stack software development lead" 
                }
            };

            await context.Set<Designation>().AddRangeAsync(designations);
            await context.SaveChangesAsync();
        }
    }
}
