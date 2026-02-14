using WebApiNibu.Data.Dto.Feed;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Contract.Feed.Events;

public interface IEvent
{
    // Queries
    Task<Result<PagedResult<EventReadDto>>> GetAllAsync(EventFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<EventReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<EventReadDto>> CreateAsync(EventCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, EventUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}