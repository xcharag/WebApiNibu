namespace WebApiNibu.Data.Dto.Feed.News;

public class NewsReactionReadDto
{


    public int Id { get; set; }

    //Foreign Keys

    public int NewsId { get; set; }

    public int UserId { get; set; }

    public int MerchId { get; set; }

    // Navigation names (added)
    public string? UserName { get; set; } = string.Empty;
    public string? MerchName { get; set; } = string.Empty;
    public string? NewsTitle { get; set; } = string.Empty;
}
public class NewsReactionUpdateDto
{



    //Foreign Keys

    public int NewsId { get; set; }

    public int UserId { get; set; }

    public int MerchId { get; set; }
}
public class NewsReactionCreateDto
{



    //Foreign Keys

    public int NewsId { get; set; }

    public int UserId { get; set; }

    public int MerchId { get; set; }
}
