using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Participation;

public static class ParticipationMapper
{
    public static ParticipationReadDto ToReadDto(Data.Entity.CopaUpsa.Participation entity) => new()
    {
        Id = entity.Id,
        Key = entity.Key,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        PhaseTypeId = entity.PhaseTypeId,
        TournamentId = entity.TournamentId,
        SchoolId = entity.SchoolId
    };

    public static Data.Entity.CopaUpsa.Participation ToEntity(ParticipationCreateDto dto) => new()
    {
        Key = dto.Key,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        PhaseTypeId = dto.PhaseTypeId,
        PhaseType = null!,
        TournamentId = dto.TournamentId,
        Tournament = null!,
        SchoolId = dto.SchoolId,
        SchoolTable = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Participation target, ParticipationUpdateDto dto)
    {
        if (dto.Key is not null) target.Key = dto.Key;
        if (dto.StartDate.HasValue) target.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue) target.EndDate = dto.EndDate.Value;
        if (dto.PhaseTypeId.HasValue) target.PhaseTypeId = dto.PhaseTypeId.Value;
        if (dto.TournamentId.HasValue) target.TournamentId = dto.TournamentId.Value;
        if (dto.SchoolId.HasValue) target.SchoolId = dto.SchoolId.Value;
    }
}

