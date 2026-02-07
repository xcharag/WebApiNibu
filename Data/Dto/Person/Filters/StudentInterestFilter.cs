namespace WebApiNibu.Data.Dto.Person.Filters;

public class StudentInterestFilter
{
    public int? SchoolStudentId { get; set; }
    public int? InterestActivityId { get; set; }
    public bool? Active { get; set; }
}
