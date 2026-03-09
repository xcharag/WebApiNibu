using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public static class TournamentRosterMapper
{
    public static TournamentRosterReadDto ToReadDto(Data.Entity.CopaUpsa.TournamentRoster entity) => new()
    {
        Id = entity.Id,
        SchoolStudentId = entity.SchoolStudentId,
        StudentName = entity.SchoolStudent is not null
            ? (entity.SchoolStudent.FirstName + " " + entity.SchoolStudent.PaternalSurname)
            : string.Empty,
        TournamentId = entity.TournamentId,
        TournamentName = entity.Tournament is not null ? entity.Tournament.Name : string.Empty,
        SchoolId = entity.SchoolId,
        SchoolName = entity.SchoolTable is not null ? entity.SchoolTable.Name : string.Empty
    };

    public static Data.Entity.CopaUpsa.TournamentRoster ToEntity(TournamentRosterCreateDto dto) => new()
    {
        SchoolStudentId = dto.SchoolStudentId,
        SchoolStudent = null!,
        TournamentId = dto.TournamentId,
        Tournament = null!,
        SchoolId = dto.SchoolId,
        SchoolTable = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.TournamentRoster target, TournamentRosterUpdateDto dto)
    {
        if (dto.SchoolStudentId.HasValue) target.SchoolStudentId = dto.SchoolStudentId.Value;
        if (dto.TournamentId.HasValue) target.TournamentId = dto.TournamentId.Value;
        if (dto.SchoolId.HasValue) target.SchoolId = dto.SchoolId.Value;
    }
}

