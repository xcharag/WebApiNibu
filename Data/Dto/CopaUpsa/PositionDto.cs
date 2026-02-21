namespace WebApiNibu.Data.Dto.CopaUpsa;

public class PositionReadDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int CoordX { get; set; }
    public int CoordY { get; set; }
}

public class PositionCreateDto
{
    public string Code { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int CoordX { get; set; }
    public int CoordY { get; set; }
}

public class PositionUpdateDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? CoordX { get; set; }
    public int? CoordY { get; set; }
}

