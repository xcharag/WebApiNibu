namespace WebApiNibu.Data.Dto.CopaUpsa;

public class TournamentRosterReadDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? MaternalName { get; set; }
    public string? DocumentNumber { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TournamentId { get; set; }
    public string? TournamentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string? SchoolName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class TournamentRosterCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? MaternalName { get; set; }
    public string? DocumentNumber { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
}

public class TournamentRosterUpdateDto
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? MaternalName { get; set; }
    public string? DocumentNumber { get; set; }
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
}

public class TournamentRosterUploadResultDto
{
    public List<TournamentRosterReadDto> Created { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}
