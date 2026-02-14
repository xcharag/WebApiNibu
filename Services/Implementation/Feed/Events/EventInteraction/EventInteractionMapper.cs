using WebApiNibu.Data.Dto.Feed.Events;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventInteraction;

public static class EventInteractionMapper
{
    public static EventInteractionReadDto ToReadDto(Data.Entity.Feed.Events.EventInteraction entity) => new()
    {
        Id = entity.Id,
        EventId = entity.EventId,
        UserId = entity.UserId,
        QrAccessId = entity.QrId ?? 0,
        MerchId = entity.MerchId ?? 0,
        isAttending = entity.IsAttending,
        NombreHermano = entity.NombreHermano ?? string.Empty,
        Origen = entity.Origen ?? string.Empty,
        IdColegio = entity.IdColegio,
        IdEstudiante = entity.IdEstudiante
    };

    public static Data.Entity.Feed.Events.EventInteraction ToEntity(EventInteractionCreateDto dto) => new()
    {
        EventId = dto.EventId,
        UserId = dto.UserId,
        QrId = dto.QrAccessId > 0 ? dto.QrAccessId : null,
        MerchId = dto.MerchId > 0 ? dto.MerchId : null,
        IsAttending = dto.isAttending,
        NombreHermano = dto.NombreHermano,
        Origen = dto.Origen,
        IdColegio = dto.IdColegio,
        IdEstudiante = dto.IdEstudiante,
        Event = null!,
        User = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Events.EventInteraction target, EventInteractionUpdateDto dto)
    {
        target.EventId = dto.EventId;
        target.UserId = dto.UserId;
        target.QrId = dto.QrAccessId > 0 ? dto.QrAccessId : null;
        target.MerchId = dto.MerchId > 0 ? dto.MerchId : null;
        target.IsAttending = dto.isAttending;
        target.NombreHermano = dto.NombreHermano;
        target.Origen = dto.Origen;
        target.IdColegio = dto.IdColegio;
        target.IdEstudiante = dto.IdEstudiante;
    }
}
