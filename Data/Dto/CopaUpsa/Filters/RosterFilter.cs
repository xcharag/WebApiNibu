namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class RosterFilter
{
    public int? MatchId { get; set; }
    public int? ParticipationId { get; set; } // Added: filter roster by participation
    public int? SchoolStudentId { get; set; }
    public int? PositionId { get; set; }
    public bool? Active { get; set; }

    // Added: filter by student name (first, paternal or maternal surname)
    public string? Name { get; set; }
}
