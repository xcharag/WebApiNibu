using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public static class MatchMapper
{
    public static MatchReadDto ToReadDto(Data.Entity.CopaUpsa.Match entity) => new()
    {
        Id = entity.Id,
        Location = entity.Location,
        ScoreA = entity.ScoreA,
        ScoreB = entity.ScoreB,
        DetailPointA = entity.DetailPointA,
        DetailPointB = entity.DetailPointB,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        NumberMatch = entity.NumberMatch,
        ParticipationAId = entity.ParticipationAId,
        ParticipationASchoolName = entity.ParticipationA?.SchoolTable?.Name ?? string.Empty,
        ParticipationBId = entity.ParticipationBId,
        ParticipationBSchoolName = entity.ParticipationB?.SchoolTable?.Name ?? string.Empty,
        MatchStatusId = entity.MatchStatusId,
        MatchStatusName = entity.MatchStatus?.Name ?? string.Empty,
        IsActive = entity.Active
    };

    public static Data.Entity.CopaUpsa.Match ToEntity(MatchCreateDto dto) => new()
    {
        Location = dto.Location,
        ScoreA = dto.ScoreA,
        ScoreB = dto.ScoreB,
        DetailPointA = dto.DetailPointA,
        DetailPointB = dto.DetailPointB,
        StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
        EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
        NumberMatch = dto.NumberMatch,
        ParticipationAId = dto.ParticipationAId,
        ParticipationA = null!,
        ParticipationBId = dto.ParticipationBId,
        ParticipationB = null!,
        MatchStatusId = dto.MatchStatusId,
        MatchStatus = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Match target, MatchUpdateDto dto)
    {
        if (dto.Location is not null) target.Location = dto.Location;
        if (dto.ScoreA.HasValue) target.ScoreA = dto.ScoreA.Value;
        if (dto.ScoreB.HasValue) target.ScoreB = dto.ScoreB.Value;
        if (dto.DetailPointA.HasValue) target.DetailPointA = dto.DetailPointA.Value;
        if (dto.DetailPointB.HasValue) target.DetailPointB = dto.DetailPointB.Value;
        if (dto.StartDate.HasValue) target.StartDate = DateTime.SpecifyKind(dto.StartDate.Value, DateTimeKind.Utc);
        if (dto.EndDate.HasValue) target.EndDate = DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Utc);
        if (dto.NumberMatch.HasValue) target.NumberMatch = dto.NumberMatch.Value;
        if (dto.ParticipationAId.HasValue) target.ParticipationAId = dto.ParticipationAId.Value;
        if (dto.ParticipationBId.HasValue) target.ParticipationBId = dto.ParticipationBId.Value;
        if (dto.MatchStatusId.HasValue) target.MatchStatusId = dto.MatchStatusId.Value;
    }
}

