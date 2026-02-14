using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsDetail;

public static class NewsDetailFilterHandler
{
    public static IQueryable<Data.Entity.Feed.News.NewsDetail> Apply(
        IQueryable<Data.Entity.Feed.News.NewsDetail> query,
        NewsDetailFilter filter)
    {
        if (filter.NewsId.HasValue)
            query = query.Where(x => x.NewsId == filter.NewsId.Value);

        return query;
    }
}
