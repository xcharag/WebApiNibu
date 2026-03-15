namespace WebApiNibu.Data.Dto.CopaUpsa;

public class StatisticEventReadDto
{
    public int Id { get; set; }
    public string Moment { get; set; }
    public int StatisticId { get; set; }
    public string StatisticName { get; set; } = string.Empty;
    public int RosterId { get; set; }
    public string RosterStudentName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class StatisticEventCreateDto
{
    public string Moment { get; set; }
    public int StatisticId { get; set; }
    public int RosterId { get; set; }
}

public class StatisticEventUpdateDto
{
    public string? Moment { get; set; }
    public int? StatisticId { get; set; }
    public int? RosterId { get; set; }
}
