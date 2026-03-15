using WebApiNibu.Data.Context;

namespace WebApiNibu.Abstraction;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Entity.FatherTable;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class BaseCrudImplementation<TEntity> : IBaseCrud<TEntity>
    where TEntity : BaseEntity
{
    private readonly CoreDbContext _db;
    private readonly DbSet<TEntity> _set;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseCrudImplementation(CoreDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _db = dbContext;
        _set = _db.Set<TEntity>();
        _httpContextAccessor = httpContextAccessor;
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
        var now = DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;
        entity.Active = true;

        var user = GetCurrentUser();
        entity.CreatedBy = user;
        entity.UpdatedBy = user;

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

        entity.UpdatedBy = GetCurrentUser();

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
            entity.UpdatedBy = GetCurrentUser();
        }
        else
        {
            _set.Remove(entity);
        }
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private string GetCurrentUser()
    {
        try
        {
            var http = _httpContextAccessor?.HttpContext;
            var user = http?.User;
            if (user is null) return string.Empty;
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrWhiteSpace(id)) return id;

            var nameClaim = user.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(nameClaim)) return nameClaim;

            var preferred = user.FindFirst("preferred_username")?.Value;
            if (!string.IsNullOrWhiteSpace(preferred)) return preferred;

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email)) return email;

            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync([id], ct);
        return entity is not null;
    }
}