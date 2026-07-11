using System;

namespace HRPayroll.Domain.Common;

/// <summary>
/// Defines auditing properties for entities.
/// </summary>
public interface IAuditable
{
    DateTime CreatedOn { get; set; }
    string? CreatedBy { get; set; }
    DateTime? LastModifiedOn { get; set; }
    string? LastModifiedBy { get; set; }
}
