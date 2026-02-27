using WebApiNibu.Data.Dto.School;

namespace WebApiNibu.Services.Implementation.School.Schools;

public static class SchoolMapper
{
    public static SchoolReadDto ToReadDto(Data.Entity.School.SchoolTable entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Tier = entity.Tier,
        Address = entity.Address,
        SportLogo = entity.SportLogo,
        NormalLogo = entity.NormalLogo,
        Rue = entity.Rue,
        City = entity.City,
        Country = entity.Country,
        Delegada = entity.Delegada,
        Tipo = entity.Tipo,
        Ciudad = entity.Ciudad,
        IdDepartamento = entity.IdDepartamento,
        Ka = entity.Ka,
        IdDelegada = entity.IdDelegada,
        IdColegio = entity.IdColegio,
        KaRectorada = entity.KaRectorada,
        Segmento = entity.Segemento,
        SchoolStudents = entity.SchoolStudents?
            .Select(s => new Data.Dto.Person.SchoolStudentReadDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                PaternalSurname = s.PaternalSurname,
                MaternalSurname = s.MaternalSurname,
                DocumentNumber = s.DocumentNumber,
                BirthDate = s.BirthDate,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                IdCountry = s.IdCountry,
                IdDocumentType = s.IdDocumentType,
                IdSchool = s.IdSchool,
                SchoolGrade = s.SchoolGrade,
                IsPlayer = s.IsPlayer,
                HasUpsaParents = s.HasUpsaParents
            }).ToList(),
        Contacts = entity.Contacts?
            .Select(c => new ContactReadDto
            {
                Id = c.Id,
                PersonName = c.PersonName,
                PersonRole = c.PersonRole,
                PersonPhoneNumber = c.PersonPhoneNumber,
                PersonEmail = c.PersonEmail
            }).ToList()
    };

    public static Data.Entity.School.SchoolTable ToEntity(SchoolCreateDto dto) => new()
    {
        Name = dto.Name,
        Tier = dto.Tier,
        Address = dto.Address,
        SportLogo = dto.SportLogo,
        NormalLogo = dto.NormalLogo,
        Rue = dto.Rue,
        City = dto.City,
        Country = dto.Country,
        Delegada = dto.Delegada,
        Tipo = dto.Tipo,
        Ciudad = dto.Ciudad,
        IdDepartamento = dto.IdDepartamento,
        Ka = dto.Ka,
        IdDelegada = dto.IdDelegada,
        IdColegio = dto.IdColegio,
        KaRectorada = dto.KaRectorada,
        Segemento = dto.Segmento,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.School.SchoolTable target, SchoolUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Tier is not null) target.Tier = dto.Tier;
        if (dto.Address is not null) target.Address = dto.Address;
        if (dto.SportLogo is not null) target.SportLogo = dto.SportLogo;
        if (dto.NormalLogo is not null) target.NormalLogo = dto.NormalLogo;
        if (dto.City is not null) target.City = dto.City;
        if (dto.Country is not null) target.Country = dto.Country;
    }
}