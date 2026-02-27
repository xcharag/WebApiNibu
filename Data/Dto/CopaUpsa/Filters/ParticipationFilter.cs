namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class ParticipationFilter
{
    public string? Key { get; set; }
    public int? PhaseTypeId { get; set; }
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
    public bool? Active { get; set; }
}

