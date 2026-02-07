using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.SchoolStudent;

public static class SchoolStudentMapper
{
    public static SchoolStudentReadDto ToReadDto(Data.Entity.Person.SchoolStudent entity) => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        MiddleName = entity.MiddleName,
        PaternalSurname = entity.PaternalSurname,
        MaternalSurname = entity.MaternalSurname,
        DocumentNumber = entity.DocumentNumber,
        BirthDate = entity.BirthDate,
        PhoneNumber = entity.PhoneNumber,
        Email = entity.Email,
        IdCountry = entity.IdCountry,
        IdDocumentType = entity.IdDocumentType,
        IdSchool = entity.IdSchool,
        SchoolGrade = entity.SchoolGrade,
        IsPlayer = entity.IsPlayer,
        HasUpsaParents = entity.HasUpsaParents
    };

    public static Data.Entity.Person.SchoolStudent ToEntity(SchoolStudentCreateDto dto) => new()
    {
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        PaternalSurname = dto.PaternalSurname,
        MaternalSurname = dto.MaternalSurname,
        DocumentNumber = dto.DocumentNumber,
        BirthDate = dto.BirthDate,
        PhoneNumber = dto.PhoneNumber,
        Email = dto.Email,
        IdCountry = dto.IdCountry,
        IdDocumentType = dto.IdDocumentType,
        IdSchool = dto.IdSchool,
        SchoolGrade = dto.SchoolGrade,
        IsPlayer = dto.IsPlayer,
        HasUpsaParents = dto.HasUpsaParents,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.SchoolStudent target, SchoolStudentUpdateDto dto)
    {
        target.FirstName = dto.FirstName;
        target.MiddleName = dto.MiddleName;
        target.PaternalSurname = dto.PaternalSurname;
        target.MaternalSurname = dto.MaternalSurname;
        target.DocumentNumber = dto.DocumentNumber;
        target.BirthDate = dto.BirthDate;
        target.PhoneNumber = dto.PhoneNumber;
        target.Email = dto.Email;
        target.IdCountry = dto.IdCountry;
        target.IdDocumentType = dto.IdDocumentType;
        target.IdSchool = dto.IdSchool;
        target.SchoolGrade = dto.SchoolGrade;
        target.IsPlayer = dto.IsPlayer;
        target.HasUpsaParents = dto.HasUpsaParents;
    }
}
