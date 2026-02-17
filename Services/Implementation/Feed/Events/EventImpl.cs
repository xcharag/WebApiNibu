using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Events;
using WebApiNibu.Services.Implementation.Feed.Events.Event;

namespace WebApiNibu.Services.Implementation.Feed.Events;

public class EventImpl(IBaseCrud<Data.Entity.Feed.Events.Event> baseCrud, CoreDbContext db)
    : IEvent
{
    private readonly EventQueries _queries = new(db);
    private readonly EventCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<EventReadDto>>> GetAllAsync(EventFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<EventReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<EventReadDto>> CreateAsync(EventCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, EventUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
