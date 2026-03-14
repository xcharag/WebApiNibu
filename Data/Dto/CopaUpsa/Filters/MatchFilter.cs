namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class MatchFilter
{
    public string? Location { get; set; }
    public string? ParticipationName { get; set; } // Search by school name across both participations
    public int? ParticipationId { get; set; } // Matches where participation is either A or B
    public int? MatchStatusId { get; set; }
    public int? NumberMatch { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public bool? Active { get; set; }

    // Added: filter matches by tournament via Participation
    public int? TournamentId { get; set; }
}
