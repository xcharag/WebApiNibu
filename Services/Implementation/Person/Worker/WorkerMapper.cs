using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.Worker;

public static class WorkerMapper
{
    public static WorkerReadDto ToReadDto(Data.Entity.Person.Worker entity) => new()
    {
        Id = entity.Id,
        WorkEmail = entity.WorkEmail,
        RoleId = entity.IdRole
    };

    public static Data.Entity.Person.Worker ToEntity(WorkerCreateDto dto) => new()
    {
        WorkEmail = dto.WorkEmail,
        IdRole = dto.RoleId,
        Active = true,
        Role = null!,
        Country = null!,
        DoucmentType = null!
    };

    public static void ApplyUpdate(Data.Entity.Person.Worker target, WorkerUpdateDto dto)
    {
        target.WorkEmail = dto.WorkEmail;
        target.IdRole = dto.RoleId;
    }
}
