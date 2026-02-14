using WebApiNibu.Data.Dto.Feed.Events.Filters;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventInteraction;

public static class EventInteractionFilterHandler
{
    public static IQueryable<Data.Entity.Feed.Events.EventInteraction> Apply(
        IQueryable<Data.Entity.Feed.Events.EventInteraction> query,
        EventInteractionFilter filter)
    {
        if (filter.IsAttending.HasValue)
            query = query.Where(x => x.IsAttending == filter.IsAttending.Value);

        if (filter.UserID.HasValue)
            query = query.Where(x => x.UserId == filter.UserID.Value);

        if (filter.MerchId.HasValue)
            query = query.Where(x => x.MerchId == filter.MerchId.Value);

        if (filter.IdColegio.HasValue)
            query = query.Where(x => x.IdColegio == filter.IdColegio.Value);

        return query;
    }
}
