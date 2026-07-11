using System;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new employee.
/// </summary>
public class CreateEmployeeDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public decimal Salary { get; set; }

    public Guid DepartmentId { get; set; }
    public Guid DesignationId { get; set; }

    public AddressDto Address { get; set; } = null!;
}
