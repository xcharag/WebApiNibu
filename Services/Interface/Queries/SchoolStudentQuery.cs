namespace WebApiNibu.Services.Interface.Queries;

public sealed class SchoolStudentQuery
{
    public int? IdSchool { get; init; }
    public int? Grade { get; init; }
    public bool? IsPlayer { get; init; }
    public bool? HasUpsaParents { get; init; }
    public int? IdCountry { get; init; }
    public int? IdDocumentType { get; init; }
    public string? Name { get; init; }
    public bool? Active { get; init; }
}
