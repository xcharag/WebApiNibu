using WebApiNibu.Data.Dto.Feed.News;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsDetail;

public static class NewsDetailMapper
{
    public static NewsDetailReadDto ToReadDto(Data.Entity.Feed.News.NewsDetail entity) => new()
    {
        Id = entity.Id,
        BlockNumber = entity.BlockNumber,
        NewsDetailTyle = (NewsDetailTyle)(int)entity.Type,
        Content = entity.Content,
        NewsId = entity.NewsId
    };

    public static Data.Entity.Feed.News.NewsDetail ToEntity(NewsDetailCreateDto dto) => new()
    {
        BlockNumber = dto.BlockNumber,
        Type = (Data.Enum.NewsDetailType)(int)dto.NewsDetailTyle,
        Content = dto.Content,
        NewsId = dto.NewsId,
        News = null!,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.Feed.News.NewsDetail target, NewsDetailUpdateDto dto)
    {
        target.BlockNumber = dto.BlockNumber;
        target.Type = (Data.Enum.NewsDetailType)(int)dto.NewsDetailTyle;
        target.Content = dto.Content;
        target.NewsId = dto.NewsId;
    }
}
