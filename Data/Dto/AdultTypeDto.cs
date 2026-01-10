namespace WebApiNibu.Data.Dto;

public class AdultTypeIdReadDto
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