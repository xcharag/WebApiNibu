using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.University;

public static class UniversityFilterHandler
{
    public static IQueryable<Data.Entity.Person.University> Apply(
        IQueryable<Data.Entity.Person.University> query, UniversityFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Sigla))
            query = query.Where(x => x.Sigla != null && x.Sigla.Contains(filter.Sigla));

        if (!string.IsNullOrWhiteSpace(filter.Dpto))
            query = query.Where(x => x.Dpto != null && x.Dpto.Contains(filter.Dpto));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
