using System;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing employee profile.
/// </summary>
public class UpdateEmployeeDto
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
    public Guid DesignationId { get; set; }

    public AddressDto Address { get; set; } = null!;

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
