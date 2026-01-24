using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Country;

public static class CountryFilterHandler
{
    public static IQueryable<Data.Entity.Person.Country> Apply(
        IQueryable<Data.Entity.Person.Country> query, 
        CountryFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
