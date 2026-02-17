using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Events;
using WebApiNibu.Services.Implementation.Feed.Events.EventInteraction;

namespace WebApiNibu.Services.Implementation.Feed.Events;

public class EventInteractionImpl(IBaseCrud<Data.Entity.Feed.Events.EventInteraction> baseCrud, CoreDbContext db)
    : IEventInteraction
{
    private readonly EventInteractionQueries _queries = new(db);
    private readonly EventInteractionCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<EventInteractionReadDto>>> GetAllAsync(EventInteractionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<EventInteractionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<EventInteractionReadDto>> CreateAsync(EventInteractionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, EventInteractionUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
