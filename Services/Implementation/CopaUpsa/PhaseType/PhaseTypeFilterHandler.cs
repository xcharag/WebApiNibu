using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

public static class PhaseTypeFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.PhaseType> Apply(
        IQueryable<Data.Entity.CopaUpsa.PhaseType> query, PhaseTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

