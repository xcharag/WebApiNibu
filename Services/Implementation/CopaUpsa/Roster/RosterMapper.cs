using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public static class RosterMapper
{
    public static RosterReadDto ToReadDto(Data.Entity.CopaUpsa.Roster entity) => new()
    {
        Id = entity.Id,
        MatchId = entity.MatchId,
        TournamentRosterId = entity.TournamentRosterId,
        PositionId = entity.PositionId,
        StudentName = entity.TournamentRoster?.SchoolStudent is not null
            ? (entity.TournamentRoster.SchoolStudent.FirstName + " " + entity.TournamentRoster.SchoolStudent.PaternalSurname)
            : string.Empty,
        MatchName = entity.Match is not null
            ? $"Match #{entity.Match.NumberMatch} - {entity.Match.StartDate:yyyy-MM-dd}"
            : string.Empty,
        PositionName = entity.Position is not null ? entity.Position.Name : string.Empty
    };

    public static Data.Entity.CopaUpsa.Roster ToEntity(RosterCreateDto dto) => new()
    {
        MatchId = dto.MatchId,
        Match = null!,
        TournamentRosterId = dto.TournamentRosterId,
        TournamentRoster = null!,
        PositionId = dto.PositionId,
        Position = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Roster target, RosterUpdateDto dto)
    {
        if (dto.MatchId.HasValue) target.MatchId = dto.MatchId.Value;
        if (dto.TournamentRosterId.HasValue) target.TournamentRosterId = dto.TournamentRosterId.Value;
        if (dto.PositionId.HasValue) target.PositionId = dto.PositionId.Value;
    }
}
