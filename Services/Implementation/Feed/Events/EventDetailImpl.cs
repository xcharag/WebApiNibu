using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Data.Dto.Feed.Events.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Events;
using WebApiNibu.Services.Implementation.Feed.Events.EventDetail;

namespace WebApiNibu.Services.Implementation.Feed.Events;

public class EventDetailImpl(IBaseCrud<Data.Entity.Feed.Events.EventDetail> baseCrud, OracleDbContext db)
    : IEventDetail
{
    private readonly EventDetailQueries _queries = new(db);
    private readonly EventDetailCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<EventDetailReadDto>>> GetAllAsync(EventDetailFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<EventDetailReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<EventDetailReadDto>> CreateAsync(EventDetailCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, EventDetailUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
