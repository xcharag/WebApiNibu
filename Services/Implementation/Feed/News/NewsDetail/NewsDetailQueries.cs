using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsDetail;

public class NewsDetailQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<NewsDetailReadDto>>> GetAllAsync(
        NewsDetailFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.NewsDetails.AsQueryable();
        query = NewsDetailFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<NewsDetailReadDto>>.Success(new PagedResult<NewsDetailReadDto>
        {
            Items = items.Select(NewsDetailMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<NewsDetailReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.NewsDetails.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<NewsDetailReadDto>.Failure($"NewsDetail with id {id} not found")
            : Result<NewsDetailReadDto>.Success(NewsDetailMapper.ToReadDto(item));
    }
}
