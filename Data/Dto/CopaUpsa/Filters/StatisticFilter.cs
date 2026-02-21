using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class StatisticFilter
{
    public string? Name { get; set; }
    public StatisticType? StatisticType { get; set; }
    public int? SportId { get; set; }
    public bool? Active { get; set; }
}

