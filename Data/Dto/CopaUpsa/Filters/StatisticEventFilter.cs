namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class StatisticEventFilter
{
    public int? StatisticId { get; set; }
    public int? RosterId { get; set; }
    public int? MatchId { get; set; } // Added: filter statistic events by match (via Roster)
    public bool? Active { get; set; }
}
