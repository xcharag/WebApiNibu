using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Position;

public static class PositionMapper
{
    public static PositionReadDto ToReadDto(Data.Entity.CopaUpsa.Position entity) => new()
    {
        Id = entity.Id,
        Code = entity.Code,
        Name = entity.Name,
        CoordX = entity.CoordX,
        CoordY = entity.CoordY
    };

    public static Data.Entity.CopaUpsa.Position ToEntity(PositionCreateDto dto) => new()
    {
        Code = dto.Code,
        Name = dto.Name,
        CoordX = dto.CoordX,
        CoordY = dto.CoordY,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Position target, PositionUpdateDto dto)
    {
        if (dto.Code is not null) target.Code = dto.Code;
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.CoordX.HasValue) target.CoordX = dto.CoordX.Value;
        if (dto.CoordY.HasValue) target.CoordY = dto.CoordY.Value;
    }
}

