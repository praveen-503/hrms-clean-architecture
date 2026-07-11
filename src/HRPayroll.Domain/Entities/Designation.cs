using System.Collections.Generic;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities;

/// <summary>
/// Represents a job title/designation within the enterprise organization.
/// </summary>
public class Designation : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Property
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
