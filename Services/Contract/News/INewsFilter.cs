namespace WebApiNibu.Data.Dto.News.Filters;

public class NewsFilter
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsPublished { get; set; }
}