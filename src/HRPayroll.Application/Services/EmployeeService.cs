using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HRPayroll.Application.Contracts.Persistence;
using HRPayroll.Application.Contracts.Services;
using HRPayroll.Application.DTOs;
using HRPayroll.Domain.Entities;

namespace HRPayroll.Application.Services;

/// <summary>
/// Business service implementation for employee management.
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<EmployeeListDto> Items, int TotalCount)> GetEmployeesAsync(
        string? search,
        Guid? departmentId,
        Guid? designationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // Define query predicates
        Expression<Func<Employee, bool>> filter = e =>
            (string.IsNullOrEmpty(search) || e.FirstName.Contains(search) || e.LastName.Contains(search) || e.Email.Contains(search)) &&
            (!departmentId.HasValue || e.DepartmentId == departmentId.Value) &&
            (!designationId.HasValue || e.DesignationId == designationId.Value);

        // Includes
        var includes = new List<Expression<Func<Employee, object>>>
        {
            e => e.Department,
            e => e.Designation
        };

        // Fetch matched list (using custom sorting/filtering)
        var allMatched = await _unitOfWork.Employees.GetAsync(
            predicate: filter,
            orderBy: q => q.OrderBy(e => e.LastName).ThenBy(e => e.FirstName),
            includes: includes,
            disableTracking: true,
            cancellationToken: cancellationToken
        );

        int totalCount = allMatched.Count;
        var pagedItems = allMatched
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IReadOnlyList<EmployeeListDto>>(pagedItems);
        return (dtos, totalCount);
    }

    /// <inheritdoc />
    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await _unitOfWork.Employees.GetByIdWithDetailsAsync(id, cancellationToken);
        if (employee == null) return null;

        return _mapper.Map<EmployeeDto>(employee);
    }

    /// <inheritdoc />
    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createDto, CancellationToken cancellationToken = default)
    {
        // Validate unique email
        var isUnique = await _unitOfWork.Employees.IsEmailUniqueAsync(createDto.Email, null, cancellationToken);
        if (!isUnique)
        {
            throw new Exception($"Email '{createDto.Email}' is already in use by another employee.");
        }

        // Validate department
        var departmentExists = await _unitOfWork.Repository<Department>().GetByIdAsync(createDto.DepartmentId, cancellationToken);
        if (departmentExists == null)
        {
            throw new Exception($"Department with ID '{createDto.DepartmentId}' does not exist.");
        }

        // Validate designation
        var designationExists = await _unitOfWork.Repository<Designation>().GetByIdAsync(createDto.DesignationId, cancellationToken);
        if (designationExists == null)
        {
            throw new Exception($"Designation with ID '{createDto.DesignationId}' does not exist.");
        }

        // Map and insert
        var employee = _mapper.Map<Employee>(createDto);
        await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch back with full details
        var savedEmployee = await _unitOfWork.Employees.GetByIdWithDetailsAsync(employee.Id, cancellationToken);
        return _mapper.Map<EmployeeDto>(savedEmployee);
    }

    /// <inheritdoc />
    public async Task UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateDto, CancellationToken cancellationToken = default)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (employee == null)
        {
            throw new Exception($"Employee with ID '{id}' was not found.");
        }

        // Validate unique email
        var isUnique = await _unitOfWork.Employees.IsEmailUniqueAsync(updateDto.Email, id, cancellationToken);
        if (!isUnique)
        {
            throw new Exception($"Email '{updateDto.Email}' is already in use by another employee.");
        }

        // Validate department
        var departmentExists = await _unitOfWork.Repository<Department>().GetByIdAsync(updateDto.DepartmentId, cancellationToken);
        if (departmentExists == null)
        {
            throw new Exception($"Department with ID '{updateDto.DepartmentId}' does not exist.");
        }

        // Validate designation
        var designationExists = await _unitOfWork.Repository<Designation>().GetByIdAsync(updateDto.DesignationId, cancellationToken);
        if (designationExists == null)
        {
            throw new Exception($"Designation with ID '{updateDto.DesignationId}' does not exist.");
        }

        // Map updates to existing entity
        _mapper.Map(updateDto, employee);

        await _unitOfWork.Employees.UpdateAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (employee == null)
        {
            throw new Exception($"Employee with ID '{id}' was not found.");
        }

        await _unitOfWork.Employees.DeleteAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
