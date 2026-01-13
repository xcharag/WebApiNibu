namespace WebApiNibu.Data.Dto.Person;

public class UniversityReadDto
{
    public int Id {get;set;}

    public string Name {get;set;} = string.Empty;

    public string Sigla {get;set;} = string.Empty;

    public string Dpto {get;set;} =  string.Empty;

    public int IdEventos {get;set;}

    public int OrdenEventos {get;set;}

    public string NivelCompetencia {get;set;} = string.Empty;

}

public class UniversityCreateDto
{
    public string Name {get;set;} = string.Empty;

    public string Sigla {get;set;} = string.Empty;

    public string Dpto {get;set;} =  string.Empty;

    public int IdEventos {get;set;}

    public int OrdenEventos {get;set;}

    public string NivelCompetencia {get;set;} = string.Empty;

}

public class UniversityUpdateDto
{
    public string Name {get;set;} = string.Empty;

    public string Sigla {get;set;} = string.Empty;

    public string Dpto {get;set;} =  string.Empty;

    public int IdEventos {get;set;}

    public int OrdenEventos {get;set;}

    public string NivelCompetencia {get;set;} = string.Empty;
}