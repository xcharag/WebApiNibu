using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.MerchType;

public static class MerchTypeFilterHandler
{
    public static IQueryable<Data.Entity.Person.MerchType> Apply(
        IQueryable<Data.Entity.Person.MerchType> query, 
        MerchTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
