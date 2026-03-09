namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class RosterFilter
{
    public int? MatchId { get; set; }
    public int? ParticipationId { get; set; }
    public int? TournamentRosterId { get; set; }
    public int? PositionId { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
}
