using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventDetail;

public class EventDetailQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<EventDetailReadDto>>> GetAllAsync(
        EventDetailFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.EventDetails.AsQueryable();
        query = EventDetailFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<EventDetailReadDto>>.Success(new PagedResult<EventDetailReadDto>
        {
            Items = items.Select(EventDetailMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<EventDetailReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.EventDetails.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<EventDetailReadDto>.Failure($"EventDetail with id {id} not found")
            : Result<EventDetailReadDto>.Success(EventDetailMapper.ToReadDto(item));
    }
}
