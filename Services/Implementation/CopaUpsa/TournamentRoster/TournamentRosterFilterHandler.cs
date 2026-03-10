using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public static class TournamentRosterFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.TournamentRoster> Apply(
        IQueryable<Data.Entity.CopaUpsa.TournamentRoster> query, TournamentRosterFilter filter)
    {
        if (filter.TournamentId.HasValue)
            query = query.Where(x => x.TournamentId == filter.TournamentId.Value);

        if (filter.SchoolId.HasValue)
            query = query.Where(x => x.SchoolId == filter.SchoolId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var pattern = $"%{filter.Name.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.FirstName, pattern) ||
                (x.MiddleName != null && EF.Functions.ILike(x.MiddleName, pattern)) ||
                EF.Functions.ILike(x.LastName, pattern) ||
                (x.MaternalName != null && EF.Functions.ILike(x.MaternalName, pattern)));
        }

        return query;
    }
}

