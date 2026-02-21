namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class TournamentFilter
{
    public string? Name { get; set; }
    public int? TournamentParentId { get; set; }
    public int? SportId { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public bool? Active { get; set; }
}

