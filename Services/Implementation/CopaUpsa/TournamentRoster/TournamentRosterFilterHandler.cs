using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public static class TournamentRosterFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.TournamentRoster> Apply(
        IQueryable<Data.Entity.CopaUpsa.TournamentRoster> query, TournamentRosterFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.SchoolStudentId == filter.SchoolStudentId.Value);

        if (filter.TournamentId.HasValue)
            query = query.Where(x => x.TournamentId == filter.TournamentId.Value);

        if (filter.SchoolId.HasValue)
            query = query.Where(x => x.SchoolId == filter.SchoolId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

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

