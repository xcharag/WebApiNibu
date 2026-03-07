using System.Linq;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public static class MatchFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Match> Apply(
        IQueryable<Data.Entity.CopaUpsa.Match> query, MatchFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Location))
            query = query.Where(x => x.Location.Contains(filter.Location));

        if (filter.ParticipationAId.HasValue)
            query = query.Where(x => x.ParticipationAId == filter.ParticipationAId.Value);

        if (filter.ParticipationBId.HasValue)
            query = query.Where(x => x.ParticipationBId == filter.ParticipationBId.Value);

        if (filter.ParticipationId.HasValue)
            query = query.Where(x => x.ParticipationAId == filter.ParticipationId.Value 
                                  || x.ParticipationBId == filter.ParticipationId.Value);

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

        var tournamentId = filter.TournamentId;
        if (tournamentId.HasValue)
            query = query.Where(x => x.ParticipationA.TournamentId == tournamentId.Value 
                                  || x.ParticipationB.TournamentId == tournamentId.Value);

        return query;
    }
}
