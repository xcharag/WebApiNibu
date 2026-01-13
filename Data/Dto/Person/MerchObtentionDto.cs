namespace WebApiNibu.Data.Dto.Person;

public class MerchObtentionReadDto
{
    public int Id {get;set;}

    public string? Reason {get;set;} = string.Empty;

    public bool Claimed {get;set;}

    //Foreign Keys

    public int SchoolStudentId {get;set;}

    public int MerchId {get;set;}


}

public class MerchObtentionCreateDto
{
    public string? Reason {get;set;} = string.Empty;

    public bool Claimed {get;set;}

    //Foreign Keys

    public int SchoolStudentId {get;set;}

    public int MerchId {get;set;}
}