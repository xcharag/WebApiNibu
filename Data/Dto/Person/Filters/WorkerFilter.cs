namespace WebApiNibu.Data.Dto.Person.Filters;

public class WorkerFilter
{
    public int? RoleId { get; set; }
    public string? WorkEmail { get; set; }
    public bool? Active { get; set; }
}
