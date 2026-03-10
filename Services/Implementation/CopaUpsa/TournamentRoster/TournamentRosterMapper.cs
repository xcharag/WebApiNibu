using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public static class TournamentRosterMapper
{
    public static TournamentRosterReadDto ToReadDto(Data.Entity.CopaUpsa.TournamentRoster entity) => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        MiddleName = entity.MiddleName,
        LastName = entity.LastName,
        MaternalName = entity.MaternalName,
        DocumentNumber = entity.DocumentNumber,
        FullName = string.Join(" ",
            new[] { entity.FirstName, entity.MiddleName, entity.LastName, entity.MaternalName }
                .Where(s => !string.IsNullOrWhiteSpace(s))),
        TournamentId = entity.TournamentId,
        TournamentName = entity.Tournament is not null ? entity.Tournament.Name : string.Empty,
        SchoolId = entity.SchoolId,
        SchoolName = entity.SchoolTable is not null ? entity.SchoolTable.Name : string.Empty
    };

    public static Data.Entity.CopaUpsa.TournamentRoster ToEntity(TournamentRosterCreateDto dto) => new()
    {
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        LastName = dto.LastName,
        MaternalName = dto.MaternalName,
        DocumentNumber = dto.DocumentNumber,
        TournamentId = dto.TournamentId,
        Tournament = null!,
        SchoolId = dto.SchoolId,
        SchoolTable = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.TournamentRoster target, TournamentRosterUpdateDto dto)
    {
        if (dto.FirstName is not null) target.FirstName = dto.FirstName;
        if (dto.MiddleName is not null) target.MiddleName = dto.MiddleName;
        if (dto.LastName is not null) target.LastName = dto.LastName;
        if (dto.MaternalName is not null) target.MaternalName = dto.MaternalName;
        if (dto.DocumentNumber is not null) target.DocumentNumber = dto.DocumentNumber;
        if (dto.TournamentId.HasValue) target.TournamentId = dto.TournamentId.Value;
        if (dto.SchoolId.HasValue) target.SchoolId = dto.SchoolId.Value;
    }
}

