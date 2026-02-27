using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Match;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class MatchImpl(IBaseCrud<Data.Entity.CopaUpsa.Match> baseCrud, CoreDbContext db)
    : IMatch
{
    private readonly MatchQueries _queries = new(db);
    private readonly MatchCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<MatchReadDto>>> GetAllAsync(MatchFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<MatchReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<MatchReadDto>> CreateAsync(MatchCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, MatchUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

