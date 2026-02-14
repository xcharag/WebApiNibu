using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Contract.Feed.Polls;

public interface IOption
{
    // Queries
    Task<Result<PagedResult<OptionReadDto>>> GetAllAsync(OptionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<OptionReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<OptionReadDto>> CreateAsync(OptionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, OptionUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
