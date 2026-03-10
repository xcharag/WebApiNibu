using Microsoft.EntityFrameworkCore;
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
            var pattern = $"%{filter.Name.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.TournamentRoster.FirstName, pattern) ||
                (x.TournamentRoster.MiddleName != null && EF.Functions.ILike(x.TournamentRoster.MiddleName, pattern)) ||
                EF.Functions.ILike(x.TournamentRoster.LastName, pattern) ||
                (x.TournamentRoster.MaternalName != null && EF.Functions.ILike(x.TournamentRoster.MaternalName, pattern)));
        }

        return query;
    }
}
