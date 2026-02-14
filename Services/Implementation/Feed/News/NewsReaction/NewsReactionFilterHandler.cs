using WebApiNibu.Data.Dto.Feed.News.Filters;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

public static class NewsReactionFilterHandler
{
    public static IQueryable<Data.Entity.Feed.News.NewsReaction> Apply(
        IQueryable<Data.Entity.Feed.News.NewsReaction> query,
        NewsReactionFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.User.Name.Contains(filter.Name));

        return query;
    }
}
