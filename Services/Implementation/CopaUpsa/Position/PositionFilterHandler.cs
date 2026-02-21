using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Position;

public static class PositionFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Position> Apply(
        IQueryable<Data.Entity.CopaUpsa.Position> query, PositionFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Code))
            query = query.Where(x => x.Code.Contains(filter.Code));

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name != null && x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

