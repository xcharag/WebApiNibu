using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Option;

public static class OptionFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Polls.Option> Apply(
        IQueryable<Data.Entity.Feed.Polls.Option> query,
        OptionFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.ParticipationId.HasValue)
            query = query.Where(x => x.ParticipationId == filter.ParticipationId.Value);

        return query;
    }
}
