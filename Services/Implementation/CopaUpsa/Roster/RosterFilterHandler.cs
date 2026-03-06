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
            query = query.Where(x => x.Match.ParticipationId == filter.ParticipationId.Value);

        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.SchoolStudentId == filter.SchoolStudentId.Value);

        if (filter.PositionId.HasValue)
            query = query.Where(x => x.PositionId == filter.PositionId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        // Added: filter by student name (search in first, paternal and maternal surname)
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim();
            query = query.Where(x => x.SchoolStudent.FirstName.Contains(name) ||
                                     x.SchoolStudent.PaternalSurname.Contains(name) ||
                                     x.SchoolStudent.MaternalSurname.Contains(name));
        }

        return query;
    }
}
