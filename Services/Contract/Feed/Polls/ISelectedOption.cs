using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Contract.Feed.Polls;

public interface ISelectedOption
{
    // Queries
    Task<Result<PagedResult<SelectedOptionReadDto>>> GetAllAsync(SelectedOptionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<SelectedOptionReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<SelectedOptionReadDto>> CreateAsync(SelectedOptionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, SelectedOptionUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}