using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Merch;

public class MerchQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<MerchReadDto>>> GetAllAsync(
        MerchFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.Merchs.AsQueryable();
        query = MerchFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<MerchReadDto>>.Success(new PagedResult<MerchReadDto>
        {
            Items = items.Select(MerchMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MerchReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Merchs.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<MerchReadDto>.Failure($"Merch with id {id} not found")
            : Result<MerchReadDto>.Success(MerchMapper.ToReadDto(item));
    }
}
