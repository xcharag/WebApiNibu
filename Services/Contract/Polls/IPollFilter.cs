namespace WebApiNibu.Data.Dto.Polls.Filters;

public class PollFilter
{
    public string? Title { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
}