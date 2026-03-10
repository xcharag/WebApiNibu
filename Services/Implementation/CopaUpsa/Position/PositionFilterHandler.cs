using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Position;

public static class PositionFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Position> Apply(
        IQueryable<Data.Entity.CopaUpsa.Position> query, PositionFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Code))
        {
            var pattern = $"%{filter.Code.Trim()}%";
            query = query.Where(x => EF.Functions.ILike(x.Code, pattern));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var pattern = $"%{filter.Name.Trim()}%";
            query = query.Where(x => x.Name != null && EF.Functions.ILike(x.Name, pattern));
        }

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

