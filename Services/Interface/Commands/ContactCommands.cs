using WebApiNibu.Data.Dto;

namespace WebApiNibu.Services.Interface.Commands;

public sealed record CreateContactCommand(
    int SchoolId,
    string PersonName,
    string PersonRole,
    string? PersonPhoneNumber,
    string? PersonEmail);

public sealed record UpdateContactCommand(
    string PersonName,
    string PersonRole,
    string? PersonPhoneNumber,
    string? PersonEmail);

public static class ContactCommandMappings
{
    public static ContactCreateDto ToDto(this CreateContactCommand cmd) => new()
    {
        PersonName = cmd.PersonName,
        PersonRole = cmd.PersonRole,
        PersonPhoneNumber = cmd.PersonPhoneNumber,
        PersonEmail = cmd.PersonEmail
    };

    public static ContactUpdateDto ToDto(this UpdateContactCommand cmd) => new()
    {
        PersonName = cmd.PersonName,
        PersonRole = cmd.PersonRole,
        PersonPhoneNumber = cmd.PersonPhoneNumber,
        PersonEmail = cmd.PersonEmail
    };
}

