namespace WebApiNibu.Data.Dto.Person;

public class CarreerReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;

    public string? BannerImage {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public int AreaFormacionId {get;set;}

    public int CodCarrRel {get;set;}

    public int OrdenEventos{get;set;}


}

public class CarreerCreateDto
{
    public string Name {get;set;} = string.Empty;

    public string? BannerImage {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public int AreaFormacionId {get;set;}

    public int CodCarrRel {get;set;}

    public int OrdenEventos{get;set;}

}

public class CarreerUpdateDto
{
    public string Name {get;set;} = string.Empty;

    public string? BannerImage {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public int AreaFormacionId {get;set;}

    public int CodCarrRel {get;set;}

    public int OrdenEventos{get;set;}
}