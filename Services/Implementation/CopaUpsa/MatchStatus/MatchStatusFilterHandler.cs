using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.MatchStatus;

public static class MatchStatusFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.MatchStatus> Apply(
        IQueryable<Data.Entity.CopaUpsa.MatchStatus> query, MatchStatusFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

