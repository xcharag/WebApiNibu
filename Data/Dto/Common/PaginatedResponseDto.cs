namespace WebApiNibu.Data.Dto.Common;

public class PaginatedResponseDto<T>
{
    public List<T> Data { get; set; } = new();
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; } = 0;
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalActiveRecords { get; set; } = 0;
    public int TotalInactiveRecords { get; set; } = 0;
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}