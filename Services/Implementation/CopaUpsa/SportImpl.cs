using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Sport;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class SportImpl(IBaseCrud<Data.Entity.CopaUpsa.Sport> baseCrud, CoreDbContext db)
    : ISport
{
    private readonly SportQueries _queries = new(db);
    private readonly SportCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<SportReadDto>>> GetAllAsync(SportFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<SportReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<SportReadDto>> CreateAsync(SportCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, SportUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

