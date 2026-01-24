using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.MerchType;

public static class MerchTypeMapper
{
    public static MerchTypeReadDto ToReadDto(Data.Entity.Person.MerchType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static Data.Entity.Person.MerchType ToEntity(MerchTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.MerchType target, MerchTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
