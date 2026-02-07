using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.Role;

namespace WebApiNibu.Services.Implementation.Person;

public class RoleImpl(IBaseCrud<Data.Entity.Person.Role> baseCrud, OracleDbContext db)
    : IRole
{
    private readonly RoleQueries _queries = new(db);
    private readonly RoleCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<RoleReadDto>>> GetAllAsync(RoleFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<RoleReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<RoleReadDto>> CreateAsync(RoleCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, RoleUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
