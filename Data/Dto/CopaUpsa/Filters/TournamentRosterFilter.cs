namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class TournamentRosterFilter
{
    public int? TournamentId { get; set; }
    public int? SchoolId { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
}

