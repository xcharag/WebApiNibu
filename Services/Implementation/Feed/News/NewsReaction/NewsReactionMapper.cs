using WebApiNibu.Data.Dto.Feed.News;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

public static class NewsReactionMapper
{
    public static NewsReactionReadDto ToReadDto(Data.Entity.Feed.News.NewsReaction entity) => new()
    {
        Id = entity.Id,
        NewsId = entity.NewsId,
        UserId = entity.UserId,
        MerchId = entity.MerchId,
        UserName = entity.User is not null && entity.User.PersonTable is not null
            ? (entity.User.PersonTable.FirstName + " " + entity.User.PersonTable.PaternalSurname)
            : string.Empty,
        MerchName = entity.Merch is not null ? entity.Merch.Name : string.Empty,
        NewsTitle = entity.News is not null ? entity.News.Title : string.Empty
    };

    public static Data.Entity.Feed.News.NewsReaction ToEntity(NewsReactionCreateDto dto) => new()
    {
        NewsId = dto.NewsId,
        UserId = dto.UserId,
        MerchId = dto.MerchId,
        News = null!,
        User = null!,
        Merch = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.News.NewsReaction target, NewsReactionUpdateDto dto)
    {
        target.NewsId = dto.NewsId;
        target.UserId = dto.UserId;
        target.MerchId = dto.MerchId;
    }
}
