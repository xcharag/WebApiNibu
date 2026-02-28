namespace WebApiNibu.Data.Dto.UsersAndAccess.Filters;

public class QrAccessFilter
{
    public string? Reason { get; set; }
    public string? Value { get; set; }
    public bool? IsUsed { get; set; }
    public bool? IsExpired { get; set; }
    public DateTime? ExpirationFrom { get; set; }
    public DateTime? ExpirationTo { get; set; }
    public bool? Active { get; set; }
}
