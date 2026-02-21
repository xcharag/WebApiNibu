using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.MatchStatus;

public static class MatchStatusMapper
{
    public static MatchStatusReadDto ToReadDto(Data.Entity.CopaUpsa.MatchStatus entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Color = entity.Color
    };

    public static Data.Entity.CopaUpsa.MatchStatus ToEntity(MatchStatusCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Color = dto.Color,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.MatchStatus target, MatchStatusUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
        if (dto.Color is not null) target.Color = dto.Color;
    }
}

