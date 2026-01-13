namespace WebApiNibu.Data.Dto.Person;

public class AdultTypeReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;
}

public class AdultTypeCreateDto
{
    public string Name {get;set;} = string.Empty;
}

public class AdultTypeUpdateDto
{
    public string Name {get; set;} = string.Empty;
}