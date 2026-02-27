using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.MatchStatus;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class MatchStatusImpl(IBaseCrud<Data.Entity.CopaUpsa.MatchStatus> baseCrud, CoreDbContext db)
    : IMatchStatus
{
    private readonly MatchStatusQueries _queries = new(db);
    private readonly MatchStatusCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<MatchStatusReadDto>>> GetAllAsync(MatchStatusFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<MatchStatusReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<MatchStatusReadDto>> CreateAsync(MatchStatusCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, MatchStatusUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

