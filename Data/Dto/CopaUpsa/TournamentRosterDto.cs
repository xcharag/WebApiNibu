namespace WebApiNibu.Data.Dto.CopaUpsa;

public class TournamentRosterReadDto
{
    public int Id { get; set; }
    public int SchoolStudentId { get; set; }
    public string? StudentName { get; set; } = string.Empty;
    public int TournamentId { get; set; }
    public string? TournamentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string? SchoolName { get; set; } = string.Empty;
}

public class TournamentRosterCreateDto
{
    public int SchoolStudentId { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
}

public class TournamentRosterUpdateDto
{
    public int? SchoolStudentId { get; set; }
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
}

