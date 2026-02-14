using WebApiNibu.Data.Dto.News;
using WebApiNibu.Data.Dto.News.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.News;

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