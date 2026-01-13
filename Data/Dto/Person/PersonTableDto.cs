namespace WebApiNibu.Data.Dto.Person;

public class PersonTableReadDto
{
    public int Id {get;set;}

    public string FirstName {get;set;} = string.Empty;

    public string? MiddleName {get;set;}

    public string PaternalSurname {get;set;} = string.Empty;

    public string MaternalSurname {get;set;} = string.Empty;

    public string? DocumentNumber {get;set;}

    public DateTime BirthDate {get;set;}

    public string? PhoneNumber {get;set;}

    public string? Email {get;set;}

    //Foreign Keys

    public int CountryId {get;set;}
    public int? DocumentTypeId {get;set;}
    
}