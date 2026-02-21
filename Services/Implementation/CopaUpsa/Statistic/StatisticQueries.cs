using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Statistic;

public class StatisticQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<StatisticReadDto>>> GetAllAsync(
        StatisticFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Statistics.AsQueryable();
        query = StatisticFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<StatisticReadDto>>.Success(new PagedResult<StatisticReadDto>
        {
            Items = items.Select(StatisticMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<StatisticReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Statistics.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<StatisticReadDto>.Failure($"Statistic with id {id} not found")
            : Result<StatisticReadDto>.Success(StatisticMapper.ToReadDto(item));
    }
}

