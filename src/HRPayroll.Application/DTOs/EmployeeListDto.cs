using System;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Application.DTOs;

/// <summary>
/// Lightweight DTO for grid/list displays.
/// </summary>
public class EmployeeListDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string DesignationTitle { get; set; } = string.Empty;
    public EmployeeStatus Status { get; set; }
    public DateTime DateOfJoining { get; set; }
}
