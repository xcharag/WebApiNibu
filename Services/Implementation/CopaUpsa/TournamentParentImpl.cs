using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.TournamentParent;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class TournamentParentImpl(IBaseCrud<Data.Entity.CopaUpsa.TournamentParent> baseCrud, CoreDbContext db)
    : ITournamentParent
{
    private readonly TournamentParentQueries _queries = new(db);
    private readonly TournamentParentCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<TournamentParentReadDto>>> GetAllAsync(TournamentParentFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<TournamentParentReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<TournamentParentReadDto>> CreateAsync(TournamentParentCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, TournamentParentUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

