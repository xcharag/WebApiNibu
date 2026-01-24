namespace WebApiNibu.Data.Dto.Person.Filters;

public class AdultFilter
{
    public int? AdultTypeId { get; set; }
    public int? SchoolStudentId { get; set; }
    public string? WorkEmail { get; set; }
    public bool? Active { get; set; }
}
