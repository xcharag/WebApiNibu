using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.AdultType;

public class AdultTypeQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<AdultTypeReadDto>>> GetAllAsync(
        AdultTypeFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.AdultTypes.AsQueryable();
        query = AdultTypeFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<AdultTypeReadDto>>.Success(new PagedResult<AdultTypeReadDto>
        {
            Items = items.Select(AdultTypeMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<AdultTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.AdultTypes.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<AdultTypeReadDto>.Failure($"AdultType with id {id} not found")
            : Result<AdultTypeReadDto>.Success(AdultTypeMapper.ToReadDto(item));
    }
}
