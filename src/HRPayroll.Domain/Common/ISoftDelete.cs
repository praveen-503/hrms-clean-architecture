using System;

namespace HRPayroll.Domain.Common;

/// <summary>
/// Defines soft delete properties for entities.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    string? DeletedBy { get; set; }
}
