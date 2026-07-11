using System;
using System.Threading;
using System.Threading.Tasks;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Application.Contracts.Persistence;

/// <summary>
/// Repository contract for employee-specific database operations.
/// </summary>
public interface IEmployeeRepository : IGenericRepository<Employee>
{
    /// <summary>
    /// Gets detailed profile data of a single employee, loading department and designation relations.
    /// </summary>
    Task<Employee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an email address is unique, ignoring a specific employee record if needed.
    /// </summary>
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
