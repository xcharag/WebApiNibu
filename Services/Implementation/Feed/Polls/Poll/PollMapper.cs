using WebApiNibu.Data.Dto.Feed.Polls;
using OptionMapperAlias = WebApiNibu.Services.Implementation.Feed.Polls.Option.OptionMapper;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public static class PollMapper
{
    public static PollReadDto ToReadDto(Data.Entity.Feed.Polls.Poll entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Question = entity.Question,
        ImageUrl = entity.ImageUrl,
        ExpirationDate = entity.ExpirationDate,
        TournamentId = entity.TournamentId,
        TournamentName = entity.Tournament is not null ? entity.Tournament.Name : string.Empty,
        Options = entity.Options
            .Where(x => x.Active)
            .Select(OptionMapperAlias.ToReadDto)
            .ToList()
    };

    public static Data.Entity.Feed.Polls.Poll ToEntity(PollCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Question = dto.Question,
        ImageUrl = dto.ImageUrl,
        ExpirationDate = dto.ExpirationDate,
        TournamentId = dto.TournamentId,
        Tournament = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Polls.Poll target, PollUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.Question = dto.Question;
        target.ImageUrl = dto.ImageUrl;
        target.ExpirationDate = dto.ExpirationDate;
        target.TournamentId = dto.TournamentId;
    }
}
