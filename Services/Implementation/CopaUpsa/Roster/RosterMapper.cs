using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public static class RosterMapper
{
    public static RosterReadDto ToReadDto(Data.Entity.CopaUpsa.Roster entity) => new()
    {
        Id = entity.Id,
        MatchId = entity.MatchId,
        SchoolStudentId = entity.SchoolStudentId,
        PositionId = entity.PositionId
    };

    public static Data.Entity.CopaUpsa.Roster ToEntity(RosterCreateDto dto) => new()
    {
        MatchId = dto.MatchId,
        Match = null!,
        SchoolStudentId = dto.SchoolStudentId,
        SchoolStudent = null!,
        PositionId = dto.PositionId,
        Position = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Roster target, RosterUpdateDto dto)
    {
        if (dto.MatchId.HasValue) target.MatchId = dto.MatchId.Value;
        if (dto.SchoolStudentId.HasValue) target.SchoolStudentId = dto.SchoolStudentId.Value;
        if (dto.PositionId.HasValue) target.PositionId = dto.PositionId.Value;
    }
}

