namespace WebApiNibu.Data.Dto.Person.Filters;

public class PreferencesStudentFilter
{
    public int? SchoolStudentId { get; set; }
    public bool? HaveVocationalTest { get; set; }
    public bool? StudyAbroad { get; set; }
    public bool? Active { get; set; }
}
