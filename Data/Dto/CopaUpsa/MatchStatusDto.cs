namespace WebApiNibu.Data.Dto.CopaUpsa;

public class MatchStatusReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class MatchStatusCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class MatchStatusUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}

