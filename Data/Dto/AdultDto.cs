namespace WebApiNibu.Dto;

public class AdultReadDto
{
    public int Id {get;set;}

    public string WorkPhoneNumber {get;set;} = string.Empty;

    public string WorkEmail {get;set;} = string.Empty;

    // Foreign Keys

    public int AdultTypeID {get;set;}

    public int SchoolStudentId {get;set;}

}

public class AdultCreateDto
{
    public string WorkPhoneNumber {get;set;} = string.Empty;

    public string WorkEmail {get;set;} = string.Empty;

    //Foreign Keys

    public int AdultTypeID {get;set;}

    public int SchoolStudentId {get;set;}


}

public class AdultUpdateDto
{
    public string WorkPhoneNumber {get;set;} = string.Empty;

    public string WorkEmail {get;set;} = string.Empty;

    //Foreign Keys

    public int AdultTypeID {get;set;}

    public int SchoolStudentId {get;set;}


}
