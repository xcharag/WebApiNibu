using WebApiNibu.Data.Dto.Person;

namespace WebApiNibu.Data.Dto.School;

public class SchoolReadDto
{
    public int Id {get;set;}
    public string Name {get;set;} = string.Empty;
    public string Tier {get;set;} = string.Empty;
    public string Address {get;set;} = string.Empty;
    public string? SportLogo {get;set;} = string.Empty;
    public string? NormalLogo {get;set;} = string.Empty;
    public int Rue {get;set;}
    public string City {get;set;} = string.Empty;
    public string Country {get;set;} = string.Empty;
    public string Delegada {get;set;} = string.Empty;
    public string Tipo {get;set;} = string.Empty;
    public string Ciudad {get;set;} = string.Empty;
    public int IdDepartamento {get;set;}
    public int Ka { get; set; }
    public int IdDelegada {get;set;}
    public int IdColegio {get;set;}
    public int KaRectorada {get;set;}
    public string? Segmento {get;set;} = string.Empty;
    public ICollection<SchoolStudentReadDto>? SchoolStudents {get;set;} = new List<SchoolStudentReadDto>();
    public ICollection<ContactReadDto>? Contacts {get;set;} = new List<ContactReadDto>();
    // public ICollection<Participation>? Participations {get;set;} = new List<Participation>();
}

public class SchoolCreateDto
{
    public string Name {get;set;} = string.Empty;
    public string Tier {get;set;} = string.Empty;
    public string Address {get;set;} = string.Empty;
    public string? SportLogo {get;set;} = string.Empty;
    public string? NormalLogo {get;set;} = string.Empty;
    public int Rue { get; set; } = 0;
    public string City {get;set;} = string.Empty;
    public string Country {get;set;} = string.Empty;
    public string Delegada {get;set;} = string.Empty;
    public string Tipo {get;set;} = string.Empty;
    public string Ciudad {get;set;} = string.Empty;
    public int IdDepartamento { get; set; } = 0;
    public int Ka { get; set; } = 0;
    public int IdDelegada { get; set; } = 0;
    public int IdColegio { get; set; } = 0;
    public int KaRectorada { get; set; } = 0;
    public string? Segmento {get;set;} = string.Empty;
}

public class SchoolUpdateDto
{
    public string? Name {get;set;} = string.Empty;
    public string? Tier {get;set;} = string.Empty;
    public string? Address {get;set;} = string.Empty;
    public string? SportLogo {get;set;} = string.Empty;
    public string? NormalLogo {get;set;} = string.Empty;
    public string? City {get;set;} = string.Empty;
    public string? Country {get;set;} = string.Empty;
}