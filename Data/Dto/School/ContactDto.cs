namespace WebApiNibu.Data.Dto.School;

public class ContactReadDto
{
    public int Id {get;set;}
    public string PersonName {get;set;} = string.Empty;
    public string PersonRole {get;set;} = string.Empty;
    public string? PersonPhoneNumber {get;set;} = string.Empty;
    public string? PersonEmail {get;set;} = string.Empty;
    public SchoolReadDto School {get;set;} = new SchoolReadDto();
}

public class ContactCreateDto
{
    public string PersonName {get;set;} = string.Empty;
    public string? PersonRole {get;set;} = string.Empty;
    public string PersonPhoneNumber {get;set;} = string.Empty;
    public string? PersonEmail {get;set;} = string.Empty;
    public SchoolReadDto School {get;set;} = new SchoolReadDto();
}

public class ContactUpdateDto
{
    public string? PersonName {get;set;} = string.Empty;
    public string? PersonRole {get;set;} = string.Empty;
    public string? PersonPhoneNumber {get;set;} = string.Empty;
    public string? PersonEmail {get;set;} = string.Empty;
}