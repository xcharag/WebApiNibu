using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.MerchType;

public class MerchTypeQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<MerchTypeReadDto>>> GetAllAsync(
        MerchTypeFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.MerchTypes.AsQueryable();
        query = MerchTypeFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<MerchTypeReadDto>>.Success(new PagedResult<MerchTypeReadDto>
        {
            Items = items.Select(MerchTypeMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MerchTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.MerchTypes.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<MerchTypeReadDto>.Failure($"MerchType with id {id} not found")
            : Result<MerchTypeReadDto>.Success(MerchTypeMapper.ToReadDto(item));
    }
}
