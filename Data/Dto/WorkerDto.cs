namespace WebApiNibu.Data.Dto;

public class WorkerReadDto
{
    public int Id {get;set;}

    public string WorkEmail {get;set;} = string.Empty;

    //Foreign Keys

    public int RoleId {get;set;}

}

public class WorkerCreateDto
{

    public string WorkEmail {get;set;} = string.Empty;

    //Foreign Keys

    public int RoleId {get;set;}

}

public class WorkerUpdateDto
{

    public string WorkEmail {get;set;} = string.Empty;

    //Foreign Keys

    public int RoleId {get;set;}

}