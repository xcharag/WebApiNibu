namespace WebApiNibu.Data.Dto.CopaUpsa;

public class ParticipationReadDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int PhaseTypeId { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
}

public class ParticipationCreateDto
{
    public string Key { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int PhaseTypeId { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
}

public class ParticipationUpdateDto
{
    public string? Key { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? PhaseTypeId { get; set; }
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
}

