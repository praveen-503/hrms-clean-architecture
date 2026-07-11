using System.Collections.Generic;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities;

/// <summary>
/// Represents a department within the enterprise.
/// </summary>
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Property
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
