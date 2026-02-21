namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class MatchFilter
{
    public string? Location { get; set; }
    public int? ParticipationId { get; set; }
    public int? MatchStatusId { get; set; }
    public int? NumberMatch { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public bool? Active { get; set; }
}

