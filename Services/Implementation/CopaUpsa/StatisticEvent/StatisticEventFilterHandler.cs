using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public static class StatisticEventFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.StatisticEvent> Apply(
        IQueryable<Data.Entity.CopaUpsa.StatisticEvent> query, StatisticEventFilter filter)
    {
        if (filter.StatisticId.HasValue)
            query = query.Where(x => x.StatisticId == filter.StatisticId.Value);

        if (filter.RosterId.HasValue)
            query = query.Where(x => x.RosterId == filter.RosterId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

