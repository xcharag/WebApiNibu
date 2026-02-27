namespace WebApiNibu.Data.Dto.CopaUpsa;

public class PhaseTypeReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class PhaseTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class PhaseTypeUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

