using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class TournamentRosterImpl(IBaseCrud<Data.Entity.CopaUpsa.TournamentRoster> baseCrud, CoreDbContext db)
    : ITournamentRoster
{
    private readonly TournamentRosterQueries _queries = new(db);
    private readonly TournamentRosterCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<TournamentRosterReadDto>>> GetAllAsync(TournamentRosterFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<TournamentRosterReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<TournamentRosterReadDto>> CreateAsync(TournamentRosterCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<TournamentRosterUploadResultDto>> UploadFromExcel(IFormFile file, CancellationToken ct)
        => _commands.UploadFromExcel(file, ct);

    public Task<Result<bool>> UpdateAsync(int id, TournamentRosterUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

