namespace WebApiNibu.Data.Dto.School.Filters;

public class ContactFilter
{
    public string? PersonName { get; set; }
    public string? PersonRole { get; set; }
    public string? PersonPhoneNumber { get; set; }
    public string? PersonEmail { get; set; }
    public ICollection<int>? SchoolsId { get; set; }
}