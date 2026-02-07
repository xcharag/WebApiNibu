using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.MerchObtention;

public static class MerchObtentionMapper
{
    public static MerchObtentionReadDto ToReadDto(Data.Entity.Person.MerchObtention entity) => new()
    {
        Id = entity.Id,
        Reason = entity.Reason,
        Claimed = entity.Claimed,
        SchoolStudentId = entity.IdSchoolStudent,
        MerchId = entity.IdMerch
    };

    public static Data.Entity.Person.MerchObtention ToEntity(MerchObtentionCreateDto dto) => new()
    {
        Reason = dto.Reason ?? string.Empty,
        Claimed = dto.Claimed,
        IdSchoolStudent = dto.SchoolStudentId,
        IdMerch = dto.MerchId,
        Active = true,
        SchoolStudent = null!,
        Merch = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.MerchObtention target, MerchObtentionCreateDto dto)
    {
        target.Reason = dto.Reason ?? string.Empty;
        target.Claimed = dto.Claimed;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdMerch = dto.MerchId;
    }
}
