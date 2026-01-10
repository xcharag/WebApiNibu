namespace WebApiNibu.Data.Dto;

public enum Rarity
{
    
}
public class MerchReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public string Icon {get;set;} = string.Empty;

    public string? Image {get;set;} = string.Empty;

    public Rarity Rarity {get;set;}

    public int MaxQuantity {get;set;}

    //Foreign Keys

    public int MerchTypeId {get;set;}
}

public class MerchCreateDto
{
    public string Name {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public string Icon {get;set;} = string.Empty;

    public string? Image {get;set;} = string.Empty;

    public Rarity Rarity {get;set;}

    public int MaxQuantity {get;set;}

    //Foreign Keys

    public int MerchTypeId {get;set;}
}

public class MerchUpdateDto
{
    public string Name {get;set;} = string.Empty;

    public string? Description {get;set;} = string.Empty;

    public string Icon {get;set;} = string.Empty;

    public string? Image {get;set;} = string.Empty;

    public Rarity Rarity {get;set;}

    public int MaxQuantity {get;set;}

    //Foreign Keys

    public int MerchTypeId {get;set;}
}