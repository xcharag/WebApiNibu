namespace WebApiNibu.Data.Dto.CopaUpsa;

public class TournamentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Logo { get; set; }
    public string? Icon { get; set; }
    public string? Banner { get; set; }
    public bool HasGroupStage { get; set; }
    public bool HasPlayOffStage { get; set; }
    public int TournamentParentId { get; set; }
    public int SportId { get; set; }
}

public class TournamentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Logo { get; set; }
    public string? Icon { get; set; }
    public string? Banner { get; set; }
    public bool HasGroupStage { get; set; } = true;
    public bool HasPlayOffStage { get; set; } = true;
    public int TournamentParentId { get; set; }
    public int SportId { get; set; }
}

public class TournamentUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Logo { get; set; }
    public string? Icon { get; set; }
    public string? Banner { get; set; }
    public bool? HasGroupStage { get; set; }
    public bool? HasPlayOffStage { get; set; }
    public int? TournamentParentId { get; set; }
    public int? SportId { get; set; }
}

