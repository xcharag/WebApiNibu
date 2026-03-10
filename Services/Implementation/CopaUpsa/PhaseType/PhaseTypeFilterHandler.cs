using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;

namespace WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

public static class PhaseTypeFilterHandler
{
    public static IQueryable<Data.Entity.CopaUpsa.PhaseType> Apply(
        IQueryable<Data.Entity.CopaUpsa.PhaseType> query, PhaseTypeFilter filter)
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

