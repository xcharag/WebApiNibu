namespace WebApiNibu.Data.Dto.CopaUpsa;

public class StatisticEventReadDto
{
    public int Id { get; set; }
    public TimeOnly Moment { get; set; }
    public int StatisticId { get; set; }
    public string StatisticName { get; set; } = string.Empty;
    public int RosterId { get; set; }
    public string RosterStudentName { get; set; } = string.Empty;
}

public class StatisticEventCreateDto
{
    public TimeOnly Moment { get; set; }
    public int StatisticId { get; set; }
    public int RosterId { get; set; }
}

public class StatisticEventUpdateDto
{
    public TimeOnly? Moment { get; set; }
    public int? StatisticId { get; set; }
    public int? RosterId { get; set; }
}

