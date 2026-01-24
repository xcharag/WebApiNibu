using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.PreferencesStudent;

public static class PreferencesStudentMapper
{
    public static PreferencesStudentReadDto ToReadDto(Data.Entity.Person.PreferencesStudent entity) => new()
    {
        Id = entity.Id,
        HaveVocationalTest = entity.HaveVocationalTest,
        StudyAbroad = entity.StudyAbroad,
        WhereHadTest = (WhereHadTest?)entity.WhereHadTest,
        LevelInformation = (LevelInformation?)entity.LevelInformation,
        SchoolStudentId = entity.IdSchoolStudent
    };

    public static Data.Entity.Person.PreferencesStudent ToEntity(PreferencesStudentCreateDto dto) => new()
    {
        HaveVocationalTest = dto.HaveVocationalTest,
        StudyAbroad = dto.StudyAbroad,
        WhereHadTest = dto.WhereHadTest.HasValue ? (Data.Enum.WhereHadTest)dto.WhereHadTest.Value : Data.Enum.WhereHadTest.School,
        LevelInformation = dto.LevelInformation.HasValue ? (Data.Enum.LevelInformation)dto.LevelInformation.Value : Data.Enum.LevelInformation.Little,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        SchoolStudent = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.PreferencesStudent target, PreferencesStudentUpdateDto dto)
    {
        target.HaveVocationalTest = dto.HaveVocationalTest;
        target.StudyAbroad = dto.StudyAbroad;
        if (dto.WhereHadTest.HasValue) target.WhereHadTest = (Data.Enum.WhereHadTest)dto.WhereHadTest.Value;
        if (dto.LevelInformation.HasValue) target.LevelInformation = (Data.Enum.LevelInformation)dto.LevelInformation.Value;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}
