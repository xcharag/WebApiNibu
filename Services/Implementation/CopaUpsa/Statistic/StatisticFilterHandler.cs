using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Statistic;

public static class StatisticFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Statistic> Apply(
        IQueryable<Data.Entity.CopaUpsa.Statistic> query, StatisticFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.StatisticType.HasValue)
            query = query.Where(x => x.StatisticType == filter.StatisticType.Value);

        if (filter.SportId.HasValue)
            query = query.Where(x => x.SportId == filter.SportId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

