using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HRPayroll.Application.DTOs;

namespace HRPayroll.Application.Contracts.Services;

/// <summary>
/// Business service interface for employee management.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Gets a list of employee profiles supporting pagination, searching, and filtering.
    /// </summary>
    Task<(IReadOnlyList<EmployeeListDto> Items, int TotalCount)> GetEmployeesAsync(
        string? search,
        Guid? departmentId,
        Guid? designationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets detailed employee profile by ID.
    /// </summary>
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new employee profile.
    /// </summary>
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing employee profile.
    /// </summary>
    Task UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes (soft deletes) an employee profile.
    /// </summary>
    Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
}
