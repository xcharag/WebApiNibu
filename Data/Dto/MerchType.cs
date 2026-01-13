namespace WebApiNibu.Data.Dto;

public class MerchTypeReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;


}

public class MerchTypeCreateDto
{
    public string name {get;set;} = string.Empty;
}

public class MerchTypeUpdateDto
{
    public string name {get;set;} = string.Empty;
}

