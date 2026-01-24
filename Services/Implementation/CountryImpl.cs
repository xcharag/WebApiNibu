using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class CountryImpl(IBaseCrud<Country> baseCrud, OracleDbContext db) : ICountry
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<PagedResult<CountryReadDto>>> GetAllAsync(CountryFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Countries.AsQueryable();
        query = ApplyFilters(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<CountryReadDto>>.Success(new PagedResult<CountryReadDto>
        {
            Items = items.Select(MapToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<CountryReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<CountryReadDto>.Failure($"Country with id {id} not found")
            : Result<CountryReadDto>.Success(MapToReadDto(item));
    }


    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<CountryReadDto>> CreateAsync(CountryCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<CountryReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CountryUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Country with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Country with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Country> ApplyFilters(IQueryable<Country> query, CountryFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static CountryReadDto MapToReadDto(Country country) => new()
    {
        Id = country.Id,
        Name = country.Name
    };

    private static Country MapFromCreateDto(CountryCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(Country target, CountryUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}