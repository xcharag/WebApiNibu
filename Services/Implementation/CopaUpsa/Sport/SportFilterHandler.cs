using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Sport;

public static class SportFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.Sport> Apply(
        IQueryable<Data.Entity.CopaUpsa.Sport> query, SportFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var pattern = $"%{filter.Name.Trim()}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

