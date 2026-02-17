using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

public class NewsReactionQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<NewsReactionReadDto>>> GetAllAsync(
        NewsReactionFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.NewsReactions.Include(x => x.User).AsQueryable();
        query = NewsReactionFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<NewsReactionReadDto>>.Success(new PagedResult<NewsReactionReadDto>
        {
            Items = items.Select(NewsReactionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<NewsReactionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.NewsReactions.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<NewsReactionReadDto>.Failure($"NewsReaction with id {id} not found")
            : Result<NewsReactionReadDto>.Success(NewsReactionMapper.ToReadDto(item));
    }
}
