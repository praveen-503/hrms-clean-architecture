using System;

namespace HRPayroll.Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// </summary>
public abstract class BaseEntity<TId> : IAuditable, ISoftDelete
{
    public TId Id { get; set; } = default!;

    // Auditable Fields
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }

    // Soft Delete Fields
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }

    // Optimistic Concurrency Token
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

/// <summary>
/// Base class for entities using Guid as the primary key.
/// </summary>
public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}
