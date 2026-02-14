using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Contract.Feed.News;

public interface INewsReaction
{
    // Queries
    Task<Result<PagedResult<NewsReactionReadDto>>> GetAllAsync(NewsReactionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<NewsReactionReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<NewsReactionReadDto>> CreateAsync(NewsReactionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, NewsReactionUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}