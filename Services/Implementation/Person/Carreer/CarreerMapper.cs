using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Carreer;

public static class CarreerMapper
{
    public static CarreerReadDto ToReadDto(Data.Entity.Person.Carreer entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        BannerImage = entity.BannerImage,
        Description = entity.Description,
        AreaFormacionId = entity.AreaFormacionId,
        CodCarrRel = entity.CodCarrRel,
        OrdenEventos = entity.OrdenEventos
    };

    public static Data.Entity.Person.Carreer ToEntity(CarreerCreateDto dto) => new()
    {
        Name = dto.Name,
        BannerImage = dto.BannerImage ?? string.Empty,
        Description = dto.Description,
        AreaFormacionId = dto.AreaFormacionId,
        CodCarrRel = dto.CodCarrRel,
        OrdenEventos = dto.OrdenEventos,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.Carreer target, CarreerUpdateDto dto)
    {
        target.Name = dto.Name;
        target.BannerImage = dto.BannerImage ?? string.Empty;
        target.Description = dto.Description;
        target.AreaFormacionId = dto.AreaFormacionId;
        target.CodCarrRel = dto.CodCarrRel;
        target.OrdenEventos = dto.OrdenEventos;
    }
}
