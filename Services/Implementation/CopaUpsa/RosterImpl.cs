using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Roster;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class RosterImpl(IBaseCrud<Data.Entity.CopaUpsa.Roster> baseCrud, CoreDbContext db)
    : IRoster
{
    private readonly RosterQueries _queries = new(db);
    private readonly RosterCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<RosterReadDto>>> GetAllAsync(RosterFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<RosterReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<RosterReadDto>> CreateAsync(RosterCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, RosterUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

