namespace WebApiNibu.Data.Dto.Person;

public class DocumentTypeReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;
}

public class DocumentTypeCreateDto
{
    public string Name {get;set;} = string.Empty;
}

public class DocumentTypeUpdateDto
{
    public string Name {get;set;} = string.Empty;
}