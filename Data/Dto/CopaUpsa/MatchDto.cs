namespace WebApiNibu.Data.Dto.CopaUpsa;

public class MatchReadDto
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal ScoreA { get; set; }
    public decimal ScoreB { get; set; }
    public decimal DetailPointA { get; set; }
    public decimal DetailPointB { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberMatch { get; set; }
    public int ParticipationId { get; set; }
    public int MatchStatusId { get; set; }
}

public class MatchCreateDto
{
    public string Location { get; set; } = string.Empty;
    public decimal ScoreA { get; set; }
    public decimal ScoreB { get; set; }
    public decimal DetailPointA { get; set; }
    public decimal DetailPointB { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberMatch { get; set; }
    public int ParticipationId { get; set; }
    public int MatchStatusId { get; set; }
}

public class MatchUpdateDto
{
    public string? Location { get; set; }
    public decimal? ScoreA { get; set; }
    public decimal? ScoreB { get; set; }
    public decimal? DetailPointA { get; set; }
    public decimal? DetailPointB { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? NumberMatch { get; set; }
    public int? ParticipationId { get; set; }
    public int? MatchStatusId { get; set; }
}

