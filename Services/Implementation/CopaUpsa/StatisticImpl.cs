using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Statistic;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class StatisticImpl(IBaseCrud<Data.Entity.CopaUpsa.Statistic> baseCrud, CoreDbContext db)
    : IStatistic
{
    private readonly StatisticQueries _queries = new(db);
    private readonly StatisticCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<StatisticReadDto>>> GetAllAsync(StatisticFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<StatisticReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<StatisticReadDto>> CreateAsync(StatisticCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, StatisticUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

