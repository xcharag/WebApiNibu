namespace WebApiNibu.Services.Interface.Queries;

public sealed class ContactQuery
{
    public int? SchoolId { get; init; }
    public string? Name { get; init; }
    public string? Role { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool? Active { get; init; }
}

