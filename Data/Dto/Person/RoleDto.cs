namespace WebApiNibu.Data.Dto.Person;

public class RoleReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;

    public string Department {get;set;} = string.Empty;

}

public class RoleCreateDto
{
    public string Name {get;set;} = string.Empty;

    public string Department {get;set;} = string.Empty;
}

public class RoleUpdateDto
{
    public string Name {get;set;} = string.Empty;

    public string Department {get;set;} = string.Empty;
}