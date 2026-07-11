using System;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Application.DTOs;

/// <summary>
/// Data transfer object for full employee details.
/// </summary>
public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public EmployeeStatus Status { get; set; }
    public decimal Salary { get; set; }

    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public Guid DesignationId { get; set; }
    public string DesignationTitle { get; set; } = string.Empty;

    public AddressDto Address { get; set; } = null!;
}
