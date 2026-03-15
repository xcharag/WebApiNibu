namespace WebApiNibu.Data.Dto.CopaUpsa;

public class MatchScheduleDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal ScoreA { get; set; }
    public decimal ScoreB { get; set; }
    public int MatchStatusId { get; set; }
    public string MatchStatusName { get; set; } = string.Empty;

    public int ParticipationAId { get; set; }
    public int TeamAId { get; set; }
    public string TeamAName { get; set; } = string.Empty;
    public string? TeamALogo { get; set; }
    public int TournamentAId { get; set; }
    public string TournamentAName { get; set; } = string.Empty;
    public int PhaseTypeAId { get; set; }
    public string PhaseTypeAName { get; set; } = string.Empty;

    public int ParticipationBId { get; set; }
    public int TeamBId { get; set; }
    public string TeamBName { get; set; } = string.Empty;
    public string? TeamBLogo { get; set; }
    public int TournamentBId { get; set; }
    public string TournamentBName { get; set; } = string.Empty;
    public int PhaseTypeBId { get; set; }
    public string PhaseTypeBName { get; set; } = string.Empty;
}
