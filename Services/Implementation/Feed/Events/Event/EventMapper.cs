using WebApiNibu.Data.Dto.Feed.Events;

namespace WebApiNibu.Services.Implementation.Feed.Events.Event;

public static class EventMapper
{
    public static EventReadDto ToReadDto(Data.Entity.Feed.Events.Event entity) => new()
    {
        id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        BannerImageUrl = entity.BannerImageUrl,
        IsFeatured = entity.IsFeatured,
        FeedImageUrl = entity.FeedImageUrl ?? string.Empty,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        IdTipo = entity.IdTipo,
        IdEstado = entity.IdEstado,
        Observacion = entity.Observacion ?? string.Empty,
        Qr = entity.Qr ?? string.Empty,
        Asistencia = entity.Asistencia,
        IdUbicacion = entity.IdUbicacion,
        IdUsuario = entity.IdUsuario,
        Unidad = entity.Unidad,
        Coresponsable1 = entity.Corresponsable1 ?? string.Empty,
        Coresponsable2 = entity.Corresponsable2 ?? string.Empty,
        CargoCorres1 = entity.CargoCorres1 ?? string.Empty,
        CargoCorres2 = entity.CargoCorres2 ?? string.Empty
    };

    public static Data.Entity.Feed.Events.Event ToEntity(EventCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        BannerImageUrl = dto.BannerImageUrl,
        IsFeatured = dto.IsFeatured,
        FeedImageUrl = dto.FeedImageUrl,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        IdTipo = dto.IdTipo,
        IdEstado = dto.IdEstado,
        Observacion = dto.Observacion,
        Qr = dto.Qr,
        Asistencia = dto.Asistencia,
        IdUbicacion = dto.IdUbicacion,
        IdUsuario = dto.IdUsuario,
        Unidad = dto.Unidad,
        Corresponsable1 = dto.Coresponsable1,
        Corresponsable2 = dto.Coresponsable2,
        CargoCorres1 = dto.CargoCorres1,
        CargoCorres2 = dto.CargoCorres2,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Events.Event target, EventUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.BannerImageUrl = dto.BannerImageUrl;
        target.IsFeatured = dto.IsFeatured;
        target.FeedImageUrl = dto.FeedImageUrl;
        target.StartDate = dto.StartDate;
        target.EndDate = dto.EndDate;
        target.IdTipo = dto.IdTipo;
        target.IdEstado = dto.IdEstado;
        target.Observacion = dto.Observacion;
        target.Qr = dto.Qr;
        target.Asistencia = dto.Asistencia;
        target.IdUbicacion = dto.IdUbicacion;
        target.IdUsuario = dto.IdUsuario;
        target.Unidad = dto.Unidad;
        target.Corresponsable1 = dto.Coresponsable1;
        target.Corresponsable2 = dto.Coresponsable2;
        target.CargoCorres1 = dto.CargoCorres1;
        target.CargoCorres2 = dto.CargoCorres2;
    }
}
