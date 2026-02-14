using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.Worker;

namespace WebApiNibu.Services.Implementation.Person;

public class WorkerImpl(IBaseCrud<Data.Entity.Person.Worker> baseCrud, CoreDbContext db)
    : IWorker
{
    private readonly WorkerQueries _queries = new(db);
    private readonly WorkerCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<WorkerReadDto>>> GetAllAsync(WorkerFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<WorkerReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<WorkerReadDto>> CreateAsync(WorkerCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, WorkerUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
