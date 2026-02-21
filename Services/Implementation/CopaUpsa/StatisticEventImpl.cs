using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class StatisticEventImpl(IBaseCrud<Data.Entity.CopaUpsa.StatisticEvent> baseCrud, CoreDbContext db)
    : IStatisticEvent
{
    private readonly StatisticEventQueries _queries = new(db);
    private readonly StatisticEventCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<StatisticEventReadDto>>> GetAllAsync(StatisticEventFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<StatisticEventReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<StatisticEventReadDto>> CreateAsync(StatisticEventCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, StatisticEventUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

