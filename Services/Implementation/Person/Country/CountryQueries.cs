using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Country;

public class CountryQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<CountryReadDto>>> GetAllAsync(
        CountryFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.Countries.AsQueryable();
        query = CountryFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<CountryReadDto>>.Success(new PagedResult<CountryReadDto>
        {
            Items = items.Select(CountryMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<CountryReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Countries.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<CountryReadDto>.Failure($"Country with id {id} not found")
            : Result<CountryReadDto>.Success(CountryMapper.ToReadDto(item));
    }
}
