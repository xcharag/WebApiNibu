using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.AdultType;

public static class AdultTypeFilterHandler
{
    public static IQueryable<Data.Entity.Person.AdultType> Apply(
        IQueryable<Data.Entity.Person.AdultType> query, 
        AdultTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
