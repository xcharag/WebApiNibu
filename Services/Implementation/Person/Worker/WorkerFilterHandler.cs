using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Worker;

public static class WorkerFilterHandler
{
    public static IQueryable<Data.Entity.Person.Worker> Apply(
        IQueryable<Data.Entity.Person.Worker> query, WorkerFilter filter)
    {
        if (filter.RoleId.HasValue)
            query = query.Where(x => x.IdRole == filter.RoleId.Value);

        if (!string.IsNullOrWhiteSpace(filter.WorkEmail))
            query = query.Where(x => x.WorkEmail.Contains(filter.WorkEmail));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
