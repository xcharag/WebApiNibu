namespace WebApiNibu.Data.Dto.Person;

public class CountryReadDto
{
    public int Id {get;set;}
    
    public string Name {get;set;} = string.Empty;
}

public class CountryCreateDto
{
    public string Name {get;set;} = string.Empty;
}

public class CountryUpdateDto
{
    public string Name {get;set;} = string.Empty;
}