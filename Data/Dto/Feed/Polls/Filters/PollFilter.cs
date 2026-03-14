namespace WebApiNibu.Data.Dto.Feed.Polls.Filters;

public class PollFilter
{


    public string? Name { get; set; }

    public string? Question { get; set; }

    // Added: allow filtering polls by tournament or by match (via participation->tournament)
    public int? TournamentId { get; set; }
    public int? MatchId { get; set; }
    public DateTime? ExpirationFrom { get; set; }
    public DateTime? ExpirationTo { get; set; }
    public bool? IsExpired { get; set; }
}
