namespace WebApiNibu.Data.Dto.Person.Filters;

public class MerchObtentionFilter
{
    public int? SchoolStudentId { get; set; }
    public int? MerchId { get; set; }
    public bool? Claimed { get; set; }
    public bool? Active { get; set; }
}
