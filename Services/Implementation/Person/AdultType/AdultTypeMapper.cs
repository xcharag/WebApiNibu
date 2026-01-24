using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.AdultType;

public static class AdultTypeMapper
{
    public static AdultTypeReadDto ToReadDto(Data.Entity.Person.AdultType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static Data.Entity.Person.AdultType ToEntity(AdultTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.AdultType target, AdultTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
