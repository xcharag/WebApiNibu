using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class AdultTypeImpl(IBaseCrud<AdultType> baseCrud, OracleDbContext db) : IAdultType
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<AdultTypeReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AdultTypeReadDto>>.Success(dtos);
    }

    public async Task<Result<AdultTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<AdultTypeReadDto>.Failure($"AdultType with id {id} not found")
            : Result<AdultTypeReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<AdultTypeReadDto>>> GetFilteredAsync(AdultTypeFilter filter, CancellationToken ct)
    {
        var query = db.AdultTypes.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AdultTypeReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<AdultTypeReadDto>> CreateAsync(AdultTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AdultTypeReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AdultTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AdultType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AdultType with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<AdultType> ApplyFilters(IQueryable<AdultType> query, AdultTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static AdultTypeReadDto MapToReadDto(AdultType type) => new()
    {
        Id = type.Id,
        Name = type.Name
    };

    private static AdultType MapFromCreateDto(AdultTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(AdultType target, AdultTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}