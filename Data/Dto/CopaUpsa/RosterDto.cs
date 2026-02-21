namespace WebApiNibu.Data.Dto.CopaUpsa;

public class RosterReadDto
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int SchoolStudentId { get; set; }
    public int PositionId { get; set; }
}

public class RosterCreateDto
{
    public int MatchId { get; set; }
    public int SchoolStudentId { get; set; }
    public int PositionId { get; set; }
}

public class RosterUpdateDto
{
    public int? MatchId { get; set; }
    public int? SchoolStudentId { get; set; }
    public int? PositionId { get; set; }
}

