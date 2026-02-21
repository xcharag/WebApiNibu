using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public static class StatisticEventMapper
{
    public static StatisticEventReadDto ToReadDto(Data.Entity.CopaUpsa.StatisticEvent entity) => new()
    {
        Id = entity.Id,
        Moment = entity.Moment,
        StatisticId = entity.StatisticId,
        RosterId = entity.RosterId
    };

    public static Data.Entity.CopaUpsa.StatisticEvent ToEntity(StatisticEventCreateDto dto) => new()
    {
        Moment = dto.Moment,
        StatisticId = dto.StatisticId,
        Statistic = null!,
        RosterId = dto.RosterId,
        Roster = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.StatisticEvent target, StatisticEventUpdateDto dto)
    {
        if (dto.Moment.HasValue) target.Moment = dto.Moment.Value;
        if (dto.StatisticId.HasValue) target.StatisticId = dto.StatisticId.Value;
        if (dto.RosterId.HasValue) target.RosterId = dto.RosterId.Value;
    }
}

