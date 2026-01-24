using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IWorker
{
    // Queries
    Task<Result<IEnumerable<WorkerReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<WorkerReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<WorkerReadDto>>> GetFilteredAsync(WorkerFilter filter, CancellationToken ct);

    // Commands
    Task<Result<WorkerReadDto>> CreateAsync(WorkerCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, WorkerUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
