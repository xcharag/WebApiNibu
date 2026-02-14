namespace WebApiNibu.Data.Dto.Feed.Polls;

public class PollReadDto
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string Question { get; set; } = String.Empty;

    public string? ImageUrl { get; set; } = String.Empty;

    //Foreign Keys

    public int TournamentId { get; set; }
}

public class PollUpdateDto
{
    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string Question { get; set; } = String.Empty;

    public string? ImageUrl { get; set; } = String.Empty;

    //Foreign Keys

    public int TournamentId { get; set; }
}
public class PollCreateDto
{
    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string Question { get; set; } = String.Empty;

    public string? ImageUrl { get; set; } = String.Empty;

    //Foreign Keys

    public int TournamentId { get; set; }
}
