using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRPayroll.Application.Contracts.Persistence;

/// <summary>
/// Defines the Unit of Work pattern interface for database transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the specialized repository for Employee operations.
    /// </summary>
    IEmployeeRepository Employees { get; }

    /// <summary>
    /// Gets a repository instance for the specified entity type.
    /// </summary>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Saves all changes made in this unit of work to the database asynchronously.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
