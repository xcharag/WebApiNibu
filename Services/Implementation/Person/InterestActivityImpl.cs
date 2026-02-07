using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.InterestActivity;

namespace WebApiNibu.Services.Implementation.Person;

public class InterestActivityImpl(IBaseCrud<Data.Entity.Person.InterestActivity> baseCrud, OracleDbContext db)
    : IInterestActivity
{
    private readonly InterestActivityQueries _queries = new(db);
    private readonly InterestActivityCommands _commands = new(baseCrud);

    // Queries
    public Task<Result<PagedResult<InterestActivitieReadDto>>> GetAllAsync(InterestActivityFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<InterestActivitieReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<InterestActivitieReadDto>> CreateAsync(InterestActivitieCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, InterestActivitieUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
