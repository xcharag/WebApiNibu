using WebApiNibu.Data.Dto;

namespace WebApiNibu.Services.Interface.Commands;

public sealed record CreateSchoolStudentCommand(
    string FirstName,
    string? MiddleName,
    string PaternalSurname,
    string MaternalSurname,
    string DocumentNumber,
    DateTime BirthDate,
    string PhoneNumber,
    string Email,
    int IdCountry,
    int IdDocumentType,
    int IdSchool,
    int SchoolGrade,
    bool IsPlayer,
    bool HasUpsaParents);

public sealed record UpdateSchoolStudentCommand(
    string FirstName,
    string? MiddleName,
    string PaternalSurname,
    string MaternalSurname,
    string DocumentNumber,
    DateTime BirthDate,
    string PhoneNumber,
    string Email,
    int IdCountry,
    int IdDocumentType,
    int IdSchool,
    int SchoolGrade,
    bool IsPlayer,
    bool HasUpsaParents);

public static class SchoolStudentCommandMappings
{
    public static SchoolStudentCreateDto ToDto(this CreateSchoolStudentCommand cmd) => new()
    {
        FirstName = cmd.FirstName,
        MiddleName = cmd.MiddleName,
        PaternalSurname = cmd.PaternalSurname,
        MaternalSurname = cmd.MaternalSurname,
        DocumentNumber = cmd.DocumentNumber,
        BirthDate = cmd.BirthDate,
        PhoneNumber = cmd.PhoneNumber,
        Email = cmd.Email,
        IdCountry = cmd.IdCountry,
        IdDocumentType = cmd.IdDocumentType,
        IdSchool = cmd.IdSchool,
        SchoolGrade = cmd.SchoolGrade,
        IsPlayer = cmd.IsPlayer,
        HasUpsaParents = cmd.HasUpsaParents
    };

    public static SchoolStudentUpdateDto ToDto(this UpdateSchoolStudentCommand cmd) => new()
    {
        FirstName = cmd.FirstName,
        MiddleName = cmd.MiddleName,
        PaternalSurname = cmd.PaternalSurname,
        MaternalSurname = cmd.MaternalSurname,
        DocumentNumber = cmd.DocumentNumber,
        BirthDate = cmd.BirthDate,
        PhoneNumber = cmd.PhoneNumber,
        Email = cmd.Email,
        IdCountry = cmd.IdCountry,
        IdDocumentType = cmd.IdDocumentType,
        IdSchool = cmd.IdSchool,
        SchoolGrade = cmd.SchoolGrade,
        IsPlayer = cmd.IsPlayer,
        HasUpsaParents = cmd.HasUpsaParents
    };
}
