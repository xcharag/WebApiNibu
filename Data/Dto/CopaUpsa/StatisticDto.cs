using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Dto.CopaUpsa;

public class StatisticReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatisticType StatisticType { get; set; }
    public int SportId { get; set; }
}

public class StatisticCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatisticType StatisticType { get; set; }
    public int SportId { get; set; }
}

public class StatisticUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public StatisticType? StatisticType { get; set; }
    public int? SportId { get; set; }
}

