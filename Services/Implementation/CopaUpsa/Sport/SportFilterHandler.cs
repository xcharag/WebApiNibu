using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Sport;

public static class SportFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Sport> Apply(
        IQueryable<Data.Entity.CopaUpsa.Sport> query, SportFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

