using System;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.ValueObjects;

namespace HRPayroll.Domain.Entities;

/// <summary>
/// Represents an employee profile in the organization.
/// </summary>
public class Employee : BaseEntity
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

    // Foreign Keys
    public Guid DepartmentId { get; set; }
    public Guid DesignationId { get; set; }

    // Navigation Properties
    public virtual Department Department { get; set; } = null!;
    public virtual Designation Designation { get; set; } = null!;

    // Owned Value Object
    public Address Address { get; set; } = null!;
}
