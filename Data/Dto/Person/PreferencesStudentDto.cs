namespace WebApiNibu.Data.Dto.Person;

public enum WhereHadTest
{
    
}

public enum LevelInformation
{
    
}

public class PreferencesStudentReadDto
{
    public int Id {get;set;}

    public bool HaveVocationalTest {get;set;}

    public bool StudyAbroad {get;set;}

    public WhereHadTest? WhereHadTest {get;set;}

    public LevelInformation? LevelInformation {get;set;}

    //Foreign Keys

    public int SchoolStudentId {get;set;}
}

public class PreferencesStudentCreateDto
{
    public bool HaveVocationalTest {get;set;}

    public bool StudyAbroad {get;set;}

    public WhereHadTest? WhereHadTest {get;set;}

    public LevelInformation? LevelInformation {get;set;}

    //Foreign Keys

    public int SchoolStudentId {get;set;}

}

public class PreferencesStudentUpdateDto
{
    public bool HaveVocationalTest {get;set;}

    public bool StudyAbroad {get;set;}

    public WhereHadTest? WhereHadTest {get;set;}

    public LevelInformation? LevelInformation {get;set;}

    //Foreign Keys

    public int SchoolStudentId{get;set;}
}