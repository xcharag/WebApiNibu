namespace WebApiNibu.Data.Dto.Person;

public class AcademicPreferenceReadDto
{
    public int Id { get; set; }

    //Foreign Keys

    public int? UniversityId { get; set; }

    public int? CarreerId { get; set; }

    public int PreferencesStudentId { get; set; }


}

public class AcademicPreferenceCreateDto
{
    //Foreign Keys

    public int? UniversityId { get; set; }

    public int? CarreerId { get; set; }

    public int PreferencesStudentId { get; set; }

}

public class AcademicPreferenceUpdateDto
{
    //Foreign Keys

    public int? UniversityId { get; set; }

    public int? CarreerId { get; set; }

    public int PreferencesStudentId { get; set; }
}