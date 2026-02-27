using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public static class MatchFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Match> Apply(
        IQueryable<Data.Entity.CopaUpsa.Match> query, MatchFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Location))
            query = query.Where(x => x.Location.Contains(filter.Location));

        if (filter.ParticipationId.HasValue)
            query = query.Where(x => x.ParticipationId == filter.ParticipationId.Value);

        if (filter.MatchStatusId.HasValue)
            query = query.Where(x => x.MatchStatusId == filter.MatchStatusId.Value);

        if (filter.NumberMatch.HasValue)
            query = query.Where(x => x.NumberMatch == filter.NumberMatch.Value);

        if (filter.StartDateFrom.HasValue)
            query = query.Where(x => x.StartDate >= filter.StartDateFrom.Value);

        if (filter.StartDateTo.HasValue)
            query = query.Where(x => x.StartDate <= filter.StartDateTo.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

