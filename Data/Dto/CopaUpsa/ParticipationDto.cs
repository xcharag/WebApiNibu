namespace WebApiNibu.Data.Dto.CopaUpsa;

public class ParticipationReadDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public int PhaseTypeId { get; set; }
    public string PhaseTypeName { get; set; } = string.Empty;
    public int TournamentId { get; set; }
    public string TournamentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string? SchoolName { get; set; } = string.Empty;
}

public class ParticipationCreateDto
{
    public string Key { get; set; } = string.Empty;
    public int PhaseTypeId { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
}

public class ParticipationUpdateDto
{
    public string? Key { get; set; }
    public int? PhaseTypeId { get; set; }
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
}
