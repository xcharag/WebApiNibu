namespace WebApiNibu.Abstraction;

using System.Linq.Expressions;

public interface IBaseCrud<TEntity>
{
    // Read/query
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null, bool asNoTracking = true);
    Task<List<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = true, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    // Create
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);

    // Update
    Task<bool> UpdateAsync(int id, Action<TEntity> applyUpdates, CancellationToken ct = default);

    // Delete (soft by default)
    Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}