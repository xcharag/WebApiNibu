using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Role;

public static class RoleFilterHandler
{
    public static IQueryable<Data.Entity.Person.Role> Apply(
        IQueryable<Data.Entity.Person.Role> query, RoleFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Department))
            query = query.Where(x => x.Department.Contains(filter.Department));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
