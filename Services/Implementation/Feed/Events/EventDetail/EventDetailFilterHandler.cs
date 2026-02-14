using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventDetail;

public static class EventDetailFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Events.EventDetail> Apply(
        IQueryable<Data.Entity.Feed.Events.EventDetail> query,
        EventDetailFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.EventDetailType)
            && Enum.TryParse<Data.Enum.EventDetailType>(filter.EventDetailType, true, out var parsedType))
        {
            query = query.Where(x => x.Type == parsedType);
        }

        return query;
    }
}
