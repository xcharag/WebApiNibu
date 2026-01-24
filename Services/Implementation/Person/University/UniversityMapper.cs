using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.University;

public static class UniversityMapper
{
    public static UniversityReadDto ToReadDto(Data.Entity.Person.University entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Sigla = entity.Sigla ?? string.Empty,
        Dpto = entity.Dpto ?? string.Empty,
        IdEventos = entity.IdEventos ?? 0,
        OrdenEventos = entity.OrdenEventos ?? 0,
        NivelCompetencia = entity.NivelCompetencia ?? string.Empty
    };

    public static Data.Entity.Person.University ToEntity(UniversityCreateDto dto) => new()
    {
        Name = dto.Name,
        Sigla = dto.Sigla,
        Dpto = dto.Dpto,
        IdEventos = dto.IdEventos,
        OrdenEventos = dto.OrdenEventos,
        NivelCompetencia = dto.NivelCompetencia,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.University target, UniversityUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Sigla = dto.Sigla;
        target.Dpto = dto.Dpto;
        target.IdEventos = dto.IdEventos;
        target.OrdenEventos = dto.OrdenEventos;
        target.NivelCompetencia = dto.NivelCompetencia;
    }
}
