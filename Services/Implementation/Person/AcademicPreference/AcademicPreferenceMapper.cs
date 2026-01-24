using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.AcademicPreference;

public static class AcademicPreferenceMapper
{
    public static AcademicPreferenceReadDto ToReadDto(Data.Entity.Person.AcademicPreference entity) => new()
    {
        Id = entity.Id,
        UniversityId = entity.IdUniversitiy,
        CarreerId = entity.IdCarreer,
        PreferencesStudentId = entity.IdPreferencesStudent
    };

    public static Data.Entity.Person.AcademicPreference ToEntity(AcademicPreferenceCreateDto dto) => new()
    {
        IdUniversitiy = dto.UniversityId ?? 0,
        IdCarreer = dto.CarreerId ?? 0,
        IdPreferencesStudent = dto.PreferencesStudentId,
        Active = true,
        PreferencesStudent = null!,
        Universitiy = null!,
        Carreer = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.AcademicPreference target, AcademicPreferenceUpdateDto dto)
    {
        target.IdUniversitiy = dto.UniversityId ?? target.IdUniversitiy;
        target.IdCarreer = dto.CarreerId ?? target.IdCarreer;
        target.IdPreferencesStudent = dto.PreferencesStudentId;
    }
}
