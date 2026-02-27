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
        ParticipationId = entity.ParticipationId,
        MatchStatusId = entity.MatchStatusId
    };

    public static Data.Entity.CopaUpsa.Match ToEntity(MatchCreateDto dto) => new()
    {
        Location = dto.Location,
        ScoreA = dto.ScoreA,
        ScoreB = dto.ScoreB,
        DetailPointA = dto.DetailPointA,
        DetailPointB = dto.DetailPointB,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        NumberMatch = dto.NumberMatch,
        ParticipationId = dto.ParticipationId,
        Participation = null!,
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
        if (dto.StartDate.HasValue) target.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue) target.EndDate = dto.EndDate.Value;
        if (dto.NumberMatch.HasValue) target.NumberMatch = dto.NumberMatch.Value;
        if (dto.ParticipationId.HasValue) target.ParticipationId = dto.ParticipationId.Value;
        if (dto.MatchStatusId.HasValue) target.MatchStatusId = dto.MatchStatusId.Value;
    }
}

