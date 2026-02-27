using WebApiNibu.Data.Dto.School;

namespace WebApiNibu.Services.Implementation.School.SchoolsContacts;

public static class SchoolsContactsMapper
{
    public static ContactReadDto ToReadDto(Data.Entity.School.Contact entity) => new()
    {
        Id = entity.Id,
        PersonName = entity.PersonName,
        PersonRole = entity.PersonRole,
        PersonPhoneNumber = entity.PersonPhoneNumber,
        PersonEmail = entity.PersonEmail,
        School = entity.SchoolTable is not null
            ? new SchoolReadDto
            {
                Id = entity.SchoolTable.Id,
                Name = entity.SchoolTable.Name,
                Tier = entity.SchoolTable.Tier,
                Address = entity.SchoolTable.Address,
                City = entity.SchoolTable.City,
                Country = entity.SchoolTable.Country
            }
            : new SchoolReadDto()
    };

    public static Data.Entity.School.Contact ToEntity(ContactCreateDto dto, Data.Entity.School.SchoolTable school) => new()
    {
        PersonName = dto.PersonName,
        PersonRole = dto.PersonRole ?? string.Empty,
        PersonPhoneNumber = dto.PersonPhoneNumber,
        PersonEmail = dto.PersonEmail,
        SchoolTable = school,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.School.Contact target, ContactUpdateDto dto)
    {
        if (dto.PersonName is not null) target.PersonName = dto.PersonName;
        if (dto.PersonRole is not null) target.PersonRole = dto.PersonRole;
        if (dto.PersonPhoneNumber is not null) target.PersonPhoneNumber = dto.PersonPhoneNumber;
        if (dto.PersonEmail is not null) target.PersonEmail = dto.PersonEmail;
    }
}