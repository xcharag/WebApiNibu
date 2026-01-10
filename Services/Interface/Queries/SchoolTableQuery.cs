namespace WebApiNibu.Services.Interface.Queries;

public sealed class SchoolTableQuery
{
    public string? Name { get; init; }
    public string? Tier { get; init; }
    public string? Ciudad { get; init; }
    public int? IdDepartamento { get; init; }
    public bool? Active { get; init; }

    public bool IncludeContacts { get; init; } = true;
    public bool IncludeStudents { get; init; } = true;
}

