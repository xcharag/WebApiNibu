using WebApiNibu.Data.Dto.Feed.Polls.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Polls.SelectedOption;

public static class SelectedOptionFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Polls.SelectedOption> Apply(
        IQueryable<Data.Entity.Feed.Polls.SelectedOption> query,
        SelectedOptionFilter filter)
    {
        if (filter.UserId.HasValue)
            query = query.Where(x => x.UserId == filter.UserId.Value);

        if (filter.OptionId.HasValue)
            query = query.Where(x => x.OptionId == filter.OptionId.Value);

        return query;
    }
}
