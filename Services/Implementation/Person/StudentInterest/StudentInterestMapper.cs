using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.StudentInterest;

public static class StudentInterestMapper
{
    public static StudentInterestReadDto ToReadDto(Data.Entity.Person.StudentInterest entity) => new()
    {
        Id = entity.Id,
        MomentSelected = (MomentSelected?)entity.MomentSelected,
        Moment = entity.Moment,
        SchoolStudentId = entity.IdSchoolStudent,
        InterestActivitieId = entity.IdInterestActivity
    };

    public static Data.Entity.Person.StudentInterest ToEntity(StudentInterestCreateDto dto) => new()
    {
        MomentSelected = dto.MomentSelected.HasValue ? (Data.Enum.MomentSelected)dto.MomentSelected.Value : Data.Enum.MomentSelected.App,
        Moment = dto.Moment,
        IdSchoolStudent = dto.SchoolStudentId,
        IdInterestActivity = dto.InterestActivitieId,
        Active = true,
        SchoolStudent = null!,
        InterestActivity = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.StudentInterest target, StudentInterestUpdateDto dto)
    {
        if (dto.MomentSelected.HasValue) target.MomentSelected = (Data.Enum.MomentSelected)dto.MomentSelected.Value;
        target.Moment = dto.Moment;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdInterestActivity = dto.InterestActivitieId;
    }
}
