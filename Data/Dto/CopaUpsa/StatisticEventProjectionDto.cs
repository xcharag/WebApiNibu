namespace WebApiNibu.Data.Dto.CopaUpsa;

public class StatisticEventTimelineDto
{
    public int Id { get; set; }
    public TimeOnly Moment { get; set; }
    public int StatisticId { get; set; }
    public string StatisticName { get; set; } = string.Empty;
    public int RosterId { get; set; }
    public int TournamentRosterId { get; set; }
    public int MatchId { get; set; }
    public int TournamentId { get; set; }
    public int SchoolId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public int SchoolStudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
}

public class StatisticEventRankingDto
{
    public int SchoolStudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public int Count { get; set; }
}
