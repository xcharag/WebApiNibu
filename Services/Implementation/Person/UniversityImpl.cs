using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.University;

namespace WebApiNibu.Services.Implementation.Person;

public class UniversityImpl : IUniversity
{
    private readonly UniversityQueries _queries;
    private readonly UniversityCommands _commands;

    public UniversityImpl(IBaseCrud<Data.Entity.Person.University> baseCrud, OracleDbContext db)
    {
        _queries = new UniversityQueries(db);
        _commands = new UniversityCommands(baseCrud);
    }

    public Task<Result<PagedResult<UniversityReadDto>>> GetAllAsync(UniversityFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<UniversityReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<UniversityReadDto>> CreateAsync(UniversityCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, UniversityUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
