using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Statistic;

public static class StatisticMapper
{
    public static StatisticReadDto ToReadDto(Data.Entity.CopaUpsa.Statistic entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        StatisticType = entity.StatisticType,
        SportId = entity.SportId
    };

    public static Data.Entity.CopaUpsa.Statistic ToEntity(StatisticCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        StatisticType = dto.StatisticType,
        SportId = dto.SportId,
        Sport = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Statistic target, StatisticUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
        if (dto.StatisticType.HasValue) target.StatisticType = dto.StatisticType.Value;
        if (dto.SportId.HasValue) target.SportId = dto.SportId.Value;
    }
}

