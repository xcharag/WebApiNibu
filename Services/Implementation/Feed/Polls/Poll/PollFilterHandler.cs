using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public static class PollFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Polls.Poll> Apply(
        IQueryable<Data.Entity.Feed.Polls.Poll> query,
        PollFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Question))
            query = query.Where(x => x.Question.Contains(filter.Question));

        return query;
    }
}
