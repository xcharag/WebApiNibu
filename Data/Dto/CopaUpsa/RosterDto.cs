namespace WebApiNibu.Data.Dto.CopaUpsa;

public class RosterReadDto
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int TournamentRosterId { get; set; }
    public int PositionId { get; set; }
    public string? StudentName { get; set; } = string.Empty;
    public string? MatchName { get; set; } = string.Empty;
    public string? PositionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class RosterCreateDto
{
    public int MatchId { get; set; }
    public int TournamentRosterId { get; set; }
    public int PositionId { get; set; }
}

public class RosterUpdateDto
{
    public int? MatchId { get; set; }
    public int? TournamentRosterId { get; set; }
    public int? PositionId { get; set; }
}
