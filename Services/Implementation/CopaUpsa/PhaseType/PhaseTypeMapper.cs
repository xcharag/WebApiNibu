using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

public static class PhaseTypeMapper
{
    public static PhaseTypeReadDto ToReadDto(Data.Entity.CopaUpsa.PhaseType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description
    };

    public static Data.Entity.CopaUpsa.PhaseType ToEntity(PhaseTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.PhaseType target, PhaseTypeUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
    }
}

