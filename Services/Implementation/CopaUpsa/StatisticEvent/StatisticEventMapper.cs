using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public static class StatisticEventMapper
{
    public static StatisticEventReadDto ToReadDto(Data.Entity.CopaUpsa.StatisticEvent entity) => new()
    {
        Id = entity.Id,
        Moment = entity.Moment,
        StatisticId = entity.StatisticId,
        StatisticName = entity.Statistic?.Name ?? string.Empty,
        RosterId = entity.RosterId,
        RosterStudentName = entity.Roster?.TournamentRoster is not null
            ? string.Join(" ",
                new[] { entity.Roster.TournamentRoster.FirstName, entity.Roster.TournamentRoster.LastName }
                    .Where(s => !string.IsNullOrWhiteSpace(s)))
            : string.Empty
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

