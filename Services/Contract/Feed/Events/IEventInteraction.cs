using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Contract.Feed.Events;

public interface IEventInteraction
{
    // Queries
    Task<Result<PagedResult<EventInteractionReadDto>>> GetAllAsync(EventInteractionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<EventInteractionReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<EventInteractionReadDto>> CreateAsync(EventInteractionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, EventInteractionUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}