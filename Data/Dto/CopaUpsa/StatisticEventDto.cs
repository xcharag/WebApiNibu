namespace WebApiNibu.Data.Dto.CopaUpsa;

public class StatisticEventReadDto
{
    public int Id { get; set; }
    public TimeOnly Moment { get; set; }
    public int StatisticId { get; set; }
    public int RosterId { get; set; }
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

