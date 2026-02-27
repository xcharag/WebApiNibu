using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class PhaseTypeImpl(IBaseCrud<Data.Entity.CopaUpsa.PhaseType> baseCrud, CoreDbContext db)
    : IPhaseType
{
    private readonly PhaseTypeQueries _queries = new(db);
    private readonly PhaseTypeCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<PhaseTypeReadDto>>> GetAllAsync(PhaseTypeFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<PhaseTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<PhaseTypeReadDto>> CreateAsync(PhaseTypeCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, PhaseTypeUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

