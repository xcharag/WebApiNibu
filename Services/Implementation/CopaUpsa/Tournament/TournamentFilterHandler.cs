using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Tournament;

public static class TournamentFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Tournament> Apply(
        IQueryable<Data.Entity.CopaUpsa.Tournament> query, TournamentFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.TournamentParentId.HasValue)
            query = query.Where(x => x.TournamentParentId == filter.TournamentParentId.Value);

        if (filter.SportId.HasValue)
            query = query.Where(x => x.SportId == filter.SportId.Value);

        if (filter.StartDateFrom.HasValue)
            query = query.Where(x => x.StartDate >= filter.StartDateFrom.Value);

        if (filter.StartDateTo.HasValue)
            query = query.Where(x => x.StartDate <= filter.StartDateTo.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

