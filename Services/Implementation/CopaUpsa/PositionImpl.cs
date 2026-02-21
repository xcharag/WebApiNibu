using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Position;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class PositionImpl(IBaseCrud<Data.Entity.CopaUpsa.Position> baseCrud, CoreDbContext db)
    : IPosition
{
    private readonly PositionQueries _queries = new(db);
    private readonly PositionCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<PositionReadDto>>> GetAllAsync(PositionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<PositionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<PositionReadDto>> CreateAsync(PositionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, PositionUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

