using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Dto.CopaUpsa;

public class TournamentParentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Category Category { get; set; }
}

public class TournamentParentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Category Category { get; set; }
}

public class TournamentParentUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Category? Category { get; set; }
}

