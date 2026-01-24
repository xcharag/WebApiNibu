using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.StudentInterest;

namespace WebApiNibu.Services.Implementation.Person;

public class StudentInterestImpl(IBaseCrud<Data.Entity.Person.StudentInterest> baseCrud, OracleDbContext db)
    : IStudentInterest
{
    private readonly StudentInterestQueries _queries = new(db);
    private readonly StudentInterestCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<StudentInterestReadDto>>> GetAllAsync(StudentInterestFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<StudentInterestReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<StudentInterestReadDto>> CreateAsync(StudentInterestCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, StudentInterestUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
