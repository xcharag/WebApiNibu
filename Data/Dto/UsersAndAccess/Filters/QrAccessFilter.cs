namespace WebApiNibu.Data.Dto.UsersAndAccess.Filters;

public class QrAccessFilter
{
    public string? Reason { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DocumentNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Relationship { get; set; }
    public string? Value { get; set; }
    public bool? IsUsed { get; set; }
    public bool? WasUpsaStudent { get; set; }
    public bool? IsExpired { get; set; }
    public DateTime? ExpirationFrom { get; set; }
    public DateTime? ExpirationTo { get; set; }
    public bool? Active { get; set; }
}
