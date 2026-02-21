using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Dto.CopaUpsa.Filters;

public class TournamentParentFilter
{
    public string? Name { get; set; }
    public Category? Category { get; set; }
    public bool? Active { get; set; }
}

