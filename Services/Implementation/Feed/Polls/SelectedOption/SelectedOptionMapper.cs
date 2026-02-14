using WebApiNibu.Data.Dto.Feed.Polls;

namespace WebApiNibu.Services.Implementation.Feed.Polls.SelectedOption;

public static class SelectedOptionMapper
{
    public static SelectedOptionReadDto ToReadDto(Data.Entity.Feed.Polls.SelectedOption entity) => new()
    {
        Id = entity.Id,
        OptionId = entity.OptionId,
        UserId = entity.UserId
    };

    public static Data.Entity.Feed.Polls.SelectedOption ToEntity(SelectedOptionCreateDto dto) => new()
    {
        OptionId = dto.OptionId,
        UserId = dto.UserId,
        Option = null!,
        User = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.Polls.SelectedOption target, SelectedOptionUpdateDto dto)
    {
        target.OptionId = dto.OptionId;
        target.UserId = dto.UserId;
    }
}
