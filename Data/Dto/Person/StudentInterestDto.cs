namespace WebApiNibu.Data.Dto.Person;

public enum MomentSelected
{
    
}
public class StudentInterestReadDto
{
    
    public int Id {get;set;}

    public MomentSelected? MomentSelected {get;set;}

    public string? Moment {get;set;}

    //Foreign keys

    public int SchoolStudentId {get;set;}

    public int InterestActivitieId {get;set;}
}   

public class StudentInterestCreateDto
{
    public MomentSelected? MomentSelected {get;set;}

    public string? Moment {get;set;}

    //Foreign keys

    public int SchoolStudentId {get;set;}

    public int InterestActivitieId {get;set;}
}   

public class StudentInterestUpdateDto
{
    public MomentSelected? MomentSelected {get;set;}

    public string? Moment {get;set;}

    //Foreign keys

    public int SchoolStudentId {get;set;}

    public int InterestActivitieId {get;set;}
}   