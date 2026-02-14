using WebApiNibu.Data.Dto.Feed.News;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

public static class NewsReactionMapper
{
    public static NewsReactionReadDto ToReadDto(Data.Entity.Feed.News.NewsReaction entity) => new()
    {
        Id = entity.Id,
        NewsId = entity.NewsId,
        UserId = entity.UserId,
        MerchId = entity.MerchId
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
