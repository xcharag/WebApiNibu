using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.MerchObtention;

public class MerchObtentionQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<MerchObtentionReadDto>>> GetAllAsync(
        MerchObtentionFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.MerchObtentions.AsQueryable();
        query = MerchObtentionFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<MerchObtentionReadDto>>.Success(new PagedResult<MerchObtentionReadDto>
        {
            Items = items.Select(MerchObtentionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.MerchObtentions.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<MerchObtentionReadDto>.Failure($"MerchObtention with id {id} not found")
            : Result<MerchObtentionReadDto>.Success(MerchObtentionMapper.ToReadDto(item));
    }
}
