using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.SchoolStudent;

namespace WebApiNibu.Services.Implementation.Person;

public class SchoolStudentImpl : ISchoolStudent
{
    private readonly SchoolStudentQueries _queries;
    private readonly SchoolStudentCommands _commands;

    public SchoolStudentImpl(IBaseCrud<Data.Entity.Person.SchoolStudent> baseCrud, OracleDbContext db)
    {
        _queries = new SchoolStudentQueries(db);
        _commands = new SchoolStudentCommands(baseCrud, db);
    }

    public Task<Result<PagedResult<SchoolStudentReadDto>>> GetAllAsync(SchoolStudentFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<SchoolStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<SchoolStudentReadDto>> CreateAsync(SchoolStudentCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, SchoolStudentUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
