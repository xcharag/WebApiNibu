using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Role;

public static class RoleMapper
{
    public static RoleReadDto ToReadDto(Data.Entity.Person.Role entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Department = entity.Department
    };

    public static Data.Entity.Person.Role ToEntity(RoleCreateDto dto) => new()
    {
        Name = dto.Name,
        Department = dto.Department,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.Role target, RoleUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Department = dto.Department;
    }
}
