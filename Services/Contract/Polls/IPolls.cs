using WebApiNibu.Data.Dto.Polls;
using WebApiNibu.Data.Dto.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Polls;

public interface IPolls
{
    // Queries
    Task<Result<PagedResult<PollReadDto>>> GetAllAsync(PollFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<PollReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<PollReadDto>> CreateAsync(PollCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, PollUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}