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

        if (filter.TournamentId.HasValue)
            query = query.Where(x => x.TournamentId == filter.TournamentId.Value);

        if (filter.ExpirationFrom.HasValue)
            query = query.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value >= filter.ExpirationFrom.Value);

        if (filter.ExpirationTo.HasValue)
            query = query.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value <= filter.ExpirationTo.Value);

        if (filter.IsExpired.HasValue)
        {
            var now = DateTime.UtcNow;
            query = filter.IsExpired.Value
                ? query.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value < now)
                : query.Where(x => !x.ExpirationDate.HasValue || x.ExpirationDate.Value >= now);
        }

        return query;
    }
}
