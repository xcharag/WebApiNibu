using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentParent;

public static class TournamentParentMapper
{
    public static TournamentParentReadDto ToReadDto(Data.Entity.CopaUpsa.TournamentParent entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Category = entity.Category
    };

    public static Data.Entity.CopaUpsa.TournamentParent ToEntity(TournamentParentCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Category = dto.Category,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.TournamentParent target, TournamentParentUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
        if (dto.Category.HasValue) target.Category = dto.Category.Value;
    }
}

