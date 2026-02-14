using WebApiNibu.Data.Dto.Feed.Events;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventDetail;

public static class EventDetailMapper
{
    public static EventDetailReadDto ToReadDto(Data.Entity.Feed.Events.EventDetail entity) => new()
    {
        Id = entity.Id,
        BlockNumber = entity.BlockNumber,
        EventDetailType = (Data.Dto.Feed.Events.EventDetailType)(int)entity.Type,
        Content = entity.Content,
        EventId = entity.EventId
    };

    public static Data.Entity.Feed.Events.EventDetail ToEntity(EventDetailCreateDto dto) => new()
    {
        BlockNumber = dto.BlockNumber,
        Type = (Data.Enum.EventDetailType)(int)dto.EventDetailType,
        Content = dto.Content,
        EventId = dto.EventId,
        Event = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Events.EventDetail target, EventDetailUpdateDto dto)
    {
        target.BlockNumber = dto.BlockNumber;
        target.Type = (Data.Enum.EventDetailType)(int)dto.EventDetailType;
        target.Content = dto.Content;
        target.EventId = dto.EventId;
    }
}
