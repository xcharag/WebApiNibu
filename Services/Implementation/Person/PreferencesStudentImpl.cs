using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.PreferencesStudent;

namespace WebApiNibu.Services.Implementation.Person;

public class PreferencesStudentImpl(IBaseCrud<Data.Entity.Person.PreferencesStudent> baseCrud, OracleDbContext db)
    : IPreferencesStudent
{
    private readonly PreferencesStudentQueries _queries = new(db);
    private readonly PreferencesStudentCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<PreferencesStudentReadDto>>> GetAllAsync(PreferencesStudentFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<PreferencesStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<PreferencesStudentReadDto>> CreateAsync(PreferencesStudentCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, PreferencesStudentUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
