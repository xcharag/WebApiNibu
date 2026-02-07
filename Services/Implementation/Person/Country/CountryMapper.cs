using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Country;

public static class CountryMapper
{
    public static CountryReadDto ToReadDto(Data.Entity.Person.Country entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static Data.Entity.Person.Country ToEntity(CountryCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.Country target, CountryUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
