using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class MerchTypeImpl(IBaseCrud<MerchType> baseCrud, OracleDbContext db) : IMerchType
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<PagedResult<MerchTypeReadDto>>> GetAllAsync(MerchTypeFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.MerchTypes.AsQueryable();
        query = ApplyFilters(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);
        return Result<PagedResult<MerchTypeReadDto>>.Success(new PagedResult<MerchTypeReadDto>
        {
            Items = items.Select(MapToReadDto), PageNumber = pagination.PageNumber, PageSize = pagination.PageSize, TotalCount = totalCount
        });
    }

    public async Task<Result<MerchTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<MerchTypeReadDto>.Failure($"MerchType with id {id} not found")
            : Result<MerchTypeReadDto>.Success(MapToReadDto(item));
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<MerchTypeReadDto>> CreateAsync(MerchTypeCreateDto dto, CancellationToken ct)
    {
        var created = await baseCrud.CreateAsync(MapFromCreateDto(dto), ct);
        return Result<MerchTypeReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchType with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<MerchType> ApplyFilters(IQueryable<MerchType> query, MerchTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static MerchTypeReadDto MapToReadDto(MerchType merchType) => new()
    {
        Id = merchType.Id,
        Name = merchType.Name
    };

    private static MerchType MapFromCreateDto(MerchTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(MerchType target, MerchTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
