using WebApiNibu.Data.Dto.Feed;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Contract.Feed.News;

public interface INews
{
    // Queries
    Task<Result<PagedResult<NewsReadDto>>> GetAllAsync(NewsFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<NewsReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<NewsReadDto>> CreateAsync(NewsCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, NewsUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}