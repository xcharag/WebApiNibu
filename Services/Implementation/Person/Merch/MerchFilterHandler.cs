using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Merch;

public static class MerchFilterHandler
{
    public static IQueryable<Data.Entity.Person.Merch> Apply(
        IQueryable<Data.Entity.Person.Merch> query, 
        MerchFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.MerchTypeId.HasValue)
            query = query.Where(x => x.IdMerchType == filter.MerchTypeId.Value);

        if (filter.Rarity.HasValue)
            query = query.Where(x => (int)x.Rarity == filter.Rarity.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
