using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HRPayroll.Domain.Entities;
using HRPayroll.Application.Contracts.Persistence;

namespace HRPayroll.Persistence.Repositories;

/// <summary>
/// Specific repository implementation for Employee operations.
/// </summary>
public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(HRPayrollDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<Employee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Employee>()
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await _context.Set<Employee>()
            .AnyAsync(e => e.Email == email && (!excludeId.HasValue || e.Id != excludeId.Value), cancellationToken);
    }
}
