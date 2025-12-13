namespace WebApiNibu.Abstraction;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Entity.FatherTable;

public class BaseCrudImplementation<TEntity> : IBaseCrud<TEntity>
    where TEntity : BaseEntity
{
    private readonly OracleDbContext _db;
    private readonly DbSet<TEntity> _set;

    public BaseCrudImplementation(OracleDbContext dbContext)
    {
        _db = dbContext;
        _set = _db.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null, bool asNoTracking = true)
    {
        var query = asNoTracking ? _set.AsNoTracking() : _set.AsQueryable();
        return predicate is null ? query : query.Where(predicate);
    }

    public async Task<List<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken ct = default)
    {
        return asNoTracking ? await _set.AsNoTracking().ToListAsync(ct) : await _set.ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = true, CancellationToken ct = default)
    {
        // Assume conventional int PK named "Id"; fall back to FindAsync
        if (asNoTracking)
        {
            var entity = await _set.FindAsync([id], ct);
            if (entity is null) return null;
            _db.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        return await _set.FindAsync([id], ct);
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.Active = true;
        await _set.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Action<TEntity> applyUpdates, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync([id], ct);
        if (entity is null) return false;
        applyUpdates(entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync([id], ct);
        if (entity is null) return false;
        if (softDelete)
        {
            entity.Active = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _set.Remove(entity);
        }
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync([id], ct);
        return entity is not null;
    }
}