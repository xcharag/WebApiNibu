using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Sport;

public static class SportMapper
{
    public static SportReadDto ToReadDto(Data.Entity.CopaUpsa.Sport entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Icon = entity.Icon
    };

    public static Data.Entity.CopaUpsa.Sport ToEntity(SportCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Icon = dto.Icon,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Sport target, SportUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
        if (dto.Icon is not null) target.Icon = dto.Icon;
    }
}

