using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Tournament;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class TournamentImpl(IBaseCrud<Data.Entity.CopaUpsa.Tournament> baseCrud, CoreDbContext db)
    : ITournament
{
    private readonly TournamentQueries _queries = new(db);
    private readonly TournamentCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<TournamentReadDto>>> GetAllAsync(TournamentFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<TournamentReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<TournamentReadDto>> CreateAsync(TournamentCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, TournamentUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

