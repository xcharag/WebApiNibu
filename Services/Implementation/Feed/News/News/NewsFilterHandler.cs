using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Implementation.Feed.News.News;

public static class NewsFilterHandler
{
    public static IQueryable<Data.Entity.Feed.News.News> Apply(
        IQueryable<Data.Entity.Feed.News.News> query,
        NewsFilter filter)
    {
        if (filter.CreatedAt.HasValue)
            query = query.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Date == filter.CreatedAt.Value.Date);

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(x => x.Title.Contains(filter.Title));

        return query;
    }
}
