using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;
using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Contract.Feed.News;

public interface INewsDetail
{
    // Queries
    Task<Result<PagedResult<NewsDetailReadDto>>> GetAllAsync(NewsDetailFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<NewsDetailReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<NewsDetailReadDto>> CreateAsync(NewsDetailCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, NewsDetailUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}