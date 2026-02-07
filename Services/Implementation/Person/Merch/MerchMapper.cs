using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Merch;

public static class MerchMapper
{
    public static MerchReadDto ToReadDto(Data.Entity.Person.Merch entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Icon = entity.Icon ?? string.Empty,
        Image = entity.Image ?? string.Empty,
        Rarity = (Rarity)entity.Rarity,
        MaxQuantity = entity.MaxQuantity,
        MerchTypeId = entity.IdMerchType
    };

    public static Data.Entity.Person.Merch ToEntity(MerchCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description ?? string.Empty,
        Icon = dto.Icon,
        Image = dto.Image,
        Rarity = (Data.Enum.Rarity)dto.Rarity,
        MaxQuantity = dto.MaxQuantity,
        IdMerchType = dto.MerchTypeId,
        Active = true,
        MerchType = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.Merch target, MerchUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description ?? string.Empty;
        target.Icon = dto.Icon;
        target.Image = dto.Image;
        target.Rarity = (Data.Enum.Rarity)dto.Rarity;
        target.MaxQuantity = dto.MaxQuantity;
        target.IdMerchType = dto.MerchTypeId;
    }
}
