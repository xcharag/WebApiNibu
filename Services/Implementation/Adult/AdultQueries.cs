using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Adult;

public class AdultQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<AdultReadDto>>> GetAllAsync(
        AdultFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.Adults.AsQueryable();
        query = AdultFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<AdultReadDto>
        {
            Items = items.Select(AdultMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<AdultReadDto>>.Success(result);
    }

    public async Task<Result<AdultReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Adults.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<AdultReadDto>.Failure($"Adult with id {id} not found")
            : Result<AdultReadDto>.Success(AdultMapper.ToReadDto(item));
    }
}
