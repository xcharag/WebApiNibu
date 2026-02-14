using WebApiNibu.Data.Dto.Feed.Polls;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Option;

public static class OptionMapper
{
    public static OptionReadDto ToReadDto(Data.Entity.Feed.Polls.Option entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Correct = entity.Correct,
        PollId = entity.PollId,
        ParticipationId = entity.ParticipationId
    };

    public static Data.Entity.Feed.Polls.Option ToEntity(OptionCreateDto dto) => new()
    {
        Name = dto.Name,
        Correct = dto.Correct,
        PollId = dto.PollId,
        ParticipationId = dto.ParticipationId,
        Poll = null!,
        Participation = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Polls.Option target, OptionUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Correct = dto.Correct;
        target.PollId = dto.PollId;
        target.ParticipationId = dto.ParticipationId;
    }
}
