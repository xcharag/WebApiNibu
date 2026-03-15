using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Participation;

public static class ParticipationMapper
{
    public static ParticipationReadDto ToReadDto(Data.Entity.CopaUpsa.Participation entity) => new()
    {
        Id = entity.Id,
        Key = entity.Key,
        PhaseTypeId = entity.PhaseTypeId,
        TournamentId = entity.TournamentId,
        PhaseTypeName = entity.PhaseType?.Name ?? string.Empty,
        TournamentName = entity.Tournament?.Name ?? string.Empty,
        SchoolId = entity.SchoolId,
        SchoolName = entity.SchoolTable?.Name ?? string.Empty,
        IsActive = entity.Active
    };

    public static Data.Entity.CopaUpsa.Participation ToEntity(ParticipationCreateDto dto) => new()
    {
        Key = dto.Key,
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
        if (dto.PhaseTypeId.HasValue) target.PhaseTypeId = dto.PhaseTypeId.Value;
        if (dto.TournamentId.HasValue) target.TournamentId = dto.TournamentId.Value;
        if (dto.SchoolId.HasValue) target.SchoolId = dto.SchoolId.Value;
    }
}

