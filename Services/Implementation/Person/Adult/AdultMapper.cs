using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Adult;

public static class AdultMapper
{
    public static AdultReadDto ToReadDto(Data.Entity.Person.Adult adult) => new()
    {
        Id = adult.Id,
        WorkPhoneNumber = adult.WorkPhoneNumber,
        WorkEmail = adult.WorkEmail,
        AdultTypeId = adult.IdAdultType,
        SchoolStudentId = adult.IdSchoolStudent
    };

    public static Data.Entity.Person.Adult ToEntity(AdultCreateDto dto) => new()
    {
        WorkPhoneNumber = dto.WorkPhoneNumber,
        WorkEmail = dto.WorkEmail,
        IdAdultType = dto.AdultTypeId,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        AdultType = null!,
        SchoolStudent = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.Adult target, AdultUpdateDto dto)
    {
        target.WorkPhoneNumber = dto.WorkPhoneNumber;
        target.WorkEmail = dto.WorkEmail;
        target.IdAdultType = dto.AdultTypeId;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}
