namespace WebApiNibu.Data.Dto.Person.Filters;

public class MerchFilter
{
    public string? Name { get; set; }
    public int? MerchTypeId { get; set; }
    public int? Rarity { get; set; }
    public bool? Active { get; set; }
}
