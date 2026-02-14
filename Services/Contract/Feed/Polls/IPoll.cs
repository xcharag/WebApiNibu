using WebApiNibu.Data.Dto.Feed;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Contract.Feed.Polls;

public interface IPoll
{
    // Queries
    Task<Result<PagedResult<PollReadDto>>> GetAllAsync(PollFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<PollReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<PollReadDto>> CreateAsync(PollCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, PollUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
