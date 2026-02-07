using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Services.Implementation.Person.InterestActivity;

public static class InterestActivityMapper
{
    public static InterestActivitieReadDto ToReadDto(Data.Entity.Person.InterestActivity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Icon = entity.Icon
    };

    public static Data.Entity.Person.InterestActivity ToEntity(InterestActivitieCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Icon = dto.Icon,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Person.InterestActivity target, InterestActivitieUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.Icon = dto.Icon;
    }
}
