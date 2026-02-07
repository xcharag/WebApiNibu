using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.DocumentType;

public static class DocumentTypeMapper
{
    public static DocumentTypeReadDto ToReadDto(Data.Entity.Person.DocumentType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static Data.Entity.Person.DocumentType ToEntity(DocumentTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.DocumentType target, DocumentTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
