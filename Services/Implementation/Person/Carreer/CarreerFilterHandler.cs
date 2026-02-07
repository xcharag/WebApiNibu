using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Carreer;

public static class CarreerFilterHandler
{
    public static IQueryable<Data.Entity.Person.Carreer> Apply(
        IQueryable<Data.Entity.Person.Carreer> query, 
        CarreerFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.AreaFormacionId.HasValue)
            query = query.Where(x => x.AreaFormacionId == filter.AreaFormacionId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
