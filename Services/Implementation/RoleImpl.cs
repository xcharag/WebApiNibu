using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class RoleImpl(IBaseCrud<Role> baseCrud, OracleDbContext db) : IRole
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<PagedResult<RoleReadDto>>> GetAllAsync(RoleFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Roles.AsQueryable();
        query = ApplyFilters(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);
        return Result<PagedResult<RoleReadDto>>.Success(new PagedResult<RoleReadDto>
        {
            Items = items.Select(MapToReadDto), PageNumber = pagination.PageNumber, PageSize = pagination.PageSize, TotalCount = totalCount
        });
    }

    public async Task<Result<RoleReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<RoleReadDto>.Failure($"Role with id {id} not found")
            : Result<RoleReadDto>.Success(MapToReadDto(item));
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<RoleReadDto>> CreateAsync(RoleCreateDto dto, CancellationToken ct)
    {
        var created = await baseCrud.CreateAsync(MapFromCreateDto(dto), ct);
        return Result<RoleReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, RoleUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Role with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Role with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Role> ApplyFilters(IQueryable<Role> query, RoleFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Department))
            query = query.Where(x => x.Department.Contains(filter.Department));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static RoleReadDto MapToReadDto(Role role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Department = role.Department
    };

    private static Role MapFromCreateDto(RoleCreateDto dto) => new()
    {
        Name = dto.Name,
        Department = dto.Department,
        Active = true
    };

    private static void ApplyUpdateDto(Role target, RoleUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Department = dto.Department;
    }
}
