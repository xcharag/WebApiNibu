using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentParent;

public static class TournamentParentFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.TournamentParent> Apply(
        IQueryable<Data.Entity.CopaUpsa.TournamentParent> query, TournamentParentFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Category.HasValue)
            query = query.Where(x => x.Category == filter.Category.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

