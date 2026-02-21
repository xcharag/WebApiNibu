using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public class StatisticEventQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<StatisticEventReadDto>>> GetAllAsync(
        StatisticEventFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.StatisticEvents.AsQueryable();
        query = StatisticEventFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<StatisticEventReadDto>>.Success(new PagedResult<StatisticEventReadDto>
        {
            Items = items.Select(StatisticEventMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<StatisticEventReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.StatisticEvents.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<StatisticEventReadDto>.Failure($"StatisticEvent with id {id} not found")
            : Result<StatisticEventReadDto>.Success(StatisticEventMapper.ToReadDto(item));
    }
}

