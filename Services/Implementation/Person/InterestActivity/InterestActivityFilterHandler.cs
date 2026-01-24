using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.InterestActivity;

public static class InterestActivityFilterHandler
{
    public static IQueryable<Data.Entity.Person.InterestActivity> Apply(
        IQueryable<Data.Entity.Person.InterestActivity> query, 
        InterestActivityFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
