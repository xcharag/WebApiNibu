namespace WebApiNibu.Data.Dto.Feed.News;

public class NewsReadDto
{


    public int Id { get; set; }

    public string Title { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public string FeedImageUrl { get; set; } = String.Empty;

}


public class NewsCreateDto
{



    public string Title { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public string FeedImageUrl { get; set; } = String.Empty;

}
public class NewsUpdateDto
{



    public string Title { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public string FeedImageUrl { get; set; } = String.Empty;

}

