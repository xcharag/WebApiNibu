using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Events.Event;

public static class EventFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Events.Event> Apply(
        IQueryable<Data.Entity.Feed.Events.Event> query,
        EventFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.IsFeatured.HasValue)
            query = query.Where(x => x.IsFeatured == filter.IsFeatured.Value);

        if (filter.StartDate.HasValue)
            query = query.Where(x => x.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(x => x.EndDate <= filter.EndDate.Value);

        return query;
    }
}
