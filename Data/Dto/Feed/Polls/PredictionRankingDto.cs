namespace WebApiNibu.Data.Dto.Feed.Polls;

public class PredictionRankingItemDto
{
    public int Pos { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Correct { get; set; }
    public int Total { get; set; }
    public int Participated { get; set; }
    public double Percentage { get; set; }
    public int Streak { get; set; }
    public int MaxStreak { get; set; }
}

public class PredictionRankingDto
{
    public IEnumerable<PredictionRankingItemDto> Ranking { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalUsers { get; set; }
    public PredictionRankingItemDto? CurrentUser { get; set; }
    public int TotalPredictionsTournament { get; set; }
    public int TotalPredictionsEvaluated { get; set; }
}
