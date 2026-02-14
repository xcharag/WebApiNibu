using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventInteraction;

public class EventInteractionQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<EventInteractionReadDto>>> GetAllAsync(
        EventInteractionFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.EventInteractions.AsQueryable();
        query = EventInteractionFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<EventInteractionReadDto>>.Success(new PagedResult<EventInteractionReadDto>
        {
            Items = items.Select(EventInteractionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<EventInteractionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.EventInteractions.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<EventInteractionReadDto>.Failure($"EventInteraction with id {id} not found")
            : Result<EventInteractionReadDto>.Success(EventInteractionMapper.ToReadDto(item));
    }
}
