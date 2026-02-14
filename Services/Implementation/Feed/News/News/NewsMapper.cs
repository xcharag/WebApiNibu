using WebApiNibu.Data.Dto.Feed.News;

namespace WebApiNibu.Services.Implementation.Feed.News.News;

public static class NewsMapper
{
    public static NewsReadDto ToReadDto(Data.Entity.Feed.News.News entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        BannerImageUrl = entity.BannerImageUrl,
        FeedImageUrl = entity.FeedImageUrl ?? string.Empty
    };

    public static Data.Entity.Feed.News.News ToEntity(NewsCreateDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        BannerImageUrl = dto.BannerImageUrl,
        FeedImageUrl = dto.FeedImageUrl,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.News.News target, NewsUpdateDto dto)
    {
        target.Title = dto.Title;
        target.Description = dto.Description;
        target.BannerImageUrl = dto.BannerImageUrl;
        target.FeedImageUrl = dto.FeedImageUrl;
    }
}
