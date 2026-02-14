namespace WebApiNibu.Data.Dto.School.Filters;

public class SchoolFilter
{
    public string? Name { get; set; }
    public string? Tier { get; set; }
    public string? Address { get; set; }
    public int? MinQuantityStudents { get; set; }
    public int? MaxQuantityStudents { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}