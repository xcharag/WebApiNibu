using WebApiNibu.Data.Dto.CopaUpsa;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Tournament;

public static class TournamentMapper
{
    public static TournamentReadDto ToReadDto(Data.Entity.CopaUpsa.Tournament entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Logo = entity.Logo,
        Icon = entity.Icon,
        Banner = entity.Banner,
        HasGroupStage = entity.HasGroupStage,
        HasPlayOffStage = entity.HasPlayOffStage,
        TournamentParentId = entity.TournamentParentId,
        SportId = entity.SportId
    };

    public static Data.Entity.CopaUpsa.Tournament ToEntity(TournamentCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        Logo = dto.Logo,
        Icon = dto.Icon,
        Banner = dto.Banner,
        HasGroupStage = dto.HasGroupStage,
        HasPlayOffStage = dto.HasPlayOffStage,
        TournamentParentId = dto.TournamentParentId,
        TournamentParent = null!,
        SportId = dto.SportId,
        Sport = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.CopaUpsa.Tournament target, TournamentUpdateDto dto)
    {
        if (dto.Name is not null) target.Name = dto.Name;
        if (dto.Description is not null) target.Description = dto.Description;
        if (dto.StartDate.HasValue) target.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue) target.EndDate = dto.EndDate.Value;
        if (dto.Logo is not null) target.Logo = dto.Logo;
        if (dto.Icon is not null) target.Icon = dto.Icon;
        if (dto.Banner is not null) target.Banner = dto.Banner;
        if (dto.HasGroupStage.HasValue) target.HasGroupStage = dto.HasGroupStage.Value;
        if (dto.HasPlayOffStage.HasValue) target.HasPlayOffStage = dto.HasPlayOffStage.Value;
        if (dto.TournamentParentId.HasValue) target.TournamentParentId = dto.TournamentParentId.Value;
        if (dto.SportId.HasValue) target.SportId = dto.SportId.Value;
    }
}

