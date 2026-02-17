using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.Event;

public class EventQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<EventReadDto>>> GetAllAsync(
        EventFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.Events.AsQueryable();
        query = EventFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<EventReadDto>>.Success(new PagedResult<EventReadDto>
        {
            Items = items.Select(EventMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<EventReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Events.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<EventReadDto>.Failure($"Event with id {id} not found")
            : Result<EventReadDto>.Success(EventMapper.ToReadDto(item));
    }
}
