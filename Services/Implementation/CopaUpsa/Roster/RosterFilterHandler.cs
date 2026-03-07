using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public static class RosterFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Roster> Apply(
        IQueryable<Data.Entity.CopaUpsa.Roster> query, RosterFilter filter)
    {
        if (filter.MatchId.HasValue)
            query = query.Where(x => x.MatchId == filter.MatchId.Value);

        if (filter.ParticipationId.HasValue)
            query = query.Where(x => x.Match.ParticipationAId == filter.ParticipationId.Value
                                  || x.Match.ParticipationBId == filter.ParticipationId.Value);

        if (filter.TournamentRosterId.HasValue)
            query = query.Where(x => x.TournamentRosterId == filter.TournamentRosterId.Value);

        if (filter.PositionId.HasValue)
            query = query.Where(x => x.PositionId == filter.PositionId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim();
            query = query.Where(x => x.TournamentRoster.SchoolStudent.FirstName.Contains(name) ||
                                     x.TournamentRoster.SchoolStudent.PaternalSurname.Contains(name) ||
                                     x.TournamentRoster.SchoolStudent.MaternalSurname.Contains(name));
        }

        return query;
    }
}
