namespace WebApiNibu.Data.Dto.Person.Filters;

public class SchoolStudentFilter
{
    public string? FirstName { get; set; }
    public string? PaternalSurname { get; set; }
    public string? Email { get; set; }
    public int? IdCountry { get; set; }
    public int? IdDocumentType { get; set; }
    public int? IdSchool { get; set; }
    public string? SchoolGrade { get; set; }
    public bool? IsPlayer { get; set; }
    public bool? Active { get; set; }
}
