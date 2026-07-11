using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using HRPayroll.Application.Contracts.Persistence;
using HRPayroll.Domain.Entities;
using HRPayroll.Persistence.Repositories;

namespace HRPayroll.Persistence;

/// <summary>
/// Unit of Work implementation utilizing Entity Framework Core.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly HRPayrollDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories;
    private bool _disposed;

    public UnitOfWork(HRPayrollDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _repositories = new ConcurrentDictionary<string, object>();
    }

    /// <inheritdoc />
    public IEmployeeRepository Employees => (IEmployeeRepository)_repositories.GetOrAdd(nameof(Employee), _ => new EmployeeRepository(_context));

    /// <inheritdoc />
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var typeName = typeof(TEntity).Name;

        return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeName, _ => new GenericRepository<TEntity>(_context));
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
