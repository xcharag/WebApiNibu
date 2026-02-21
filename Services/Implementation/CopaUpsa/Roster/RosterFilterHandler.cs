using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public static class RosterFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Roster> Apply(
        IQueryable<Data.Entity.CopaUpsa.Roster> query, RosterFilter filter)
    {
        if (filter.MatchId.HasValue)
            query = query.Where(x => x.MatchId == filter.MatchId.Value);

        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.SchoolStudentId == filter.SchoolStudentId.Value);

        if (filter.PositionId.HasValue)
            query = query.Where(x => x.PositionId == filter.PositionId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

