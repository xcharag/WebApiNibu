using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Contract.Feed.Events;

public interface IEventDetail
{
    // Queries
    Task<Result<PagedResult<EventDetailReadDto>>> GetAllAsync(EventDetailFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<EventDetailReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<EventDetailReadDto>> CreateAsync(EventDetailCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, EventDetailUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}