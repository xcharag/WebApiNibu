namespace WebApiNibu.Data.Dto.Person.Filters;

public class AcademicPreferenceFilter
{
    public int? UniversityId { get; set; }
    public int? CarreerId { get; set; }
    public int? PreferencesStudentId { get; set; }
    public bool? Active { get; set; }
}
