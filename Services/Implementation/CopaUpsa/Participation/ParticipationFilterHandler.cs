using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Participation;

public static class ParticipationFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Participation> Apply(
        IQueryable<Data.Entity.CopaUpsa.Participation> query, ParticipationFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Key))
            query = query.Where(x => x.Key.Contains(filter.Key));

        if (filter.PhaseTypeId.HasValue)
            query = query.Where(x => x.PhaseTypeId == filter.PhaseTypeId.Value);

        if (filter.TournamentId.HasValue)
            query = query.Where(x => x.TournamentId == filter.TournamentId.Value);

        if (filter.SchoolId.HasValue)
            query = query.Where(x => x.SchoolId == filter.SchoolId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

