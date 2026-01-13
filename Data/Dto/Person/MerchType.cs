namespace WebApiNibu.Data.Dto.Person;

public class MerchTypeReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;


}

public class MerchTypeCreateDto
{
    public string Name {get;set;} = string.Empty;
}

public class MerchTypeUpdateDto
{
    public string Name {get;set;} = string.Empty;
}

