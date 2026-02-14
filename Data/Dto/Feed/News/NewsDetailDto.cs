namespace WebApiNibu.Data.Dto.Feed.News;
public enum NewsDetailTyle
{

}
public class NewsDetailReadDto
{


    public int Id { get; set; }

    public int BlockNumber { get; set; }

    public NewsDetailTyle NewsDetailTyle { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys


    public int NewsId { get; set; }


}


public class NewsDetailCreateDto
{



    public int BlockNumber { get; set; }

    public NewsDetailTyle NewsDetailTyle { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys


    public int NewsId { get; set; }


}
public class NewsDetailUpdateDto
{



    public int BlockNumber { get; set; }

    public NewsDetailTyle NewsDetailTyle { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys


    public int NewsId { get; set; }


}
