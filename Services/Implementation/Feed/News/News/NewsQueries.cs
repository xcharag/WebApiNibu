using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.News;

public class NewsQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<NewsReadDto>>> GetAllAsync(
        NewsFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.News.AsQueryable();
        query = NewsFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<NewsReadDto>>.Success(new PagedResult<NewsReadDto>
        {
            Items = items.Select(NewsMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<NewsReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.News.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<NewsReadDto>.Failure($"News with id {id} not found")
            : Result<NewsReadDto>.Success(NewsMapper.ToReadDto(item));
    }
}
