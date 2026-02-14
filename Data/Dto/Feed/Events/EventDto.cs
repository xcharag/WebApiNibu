namespace WebApiNibu.Data.Dto.Feed.Events;

public class EventReadDto
{

    public int id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public bool IsFeatured { get; set; }

    public string FeedImageUrl { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    //Entidades de integracion con sistema UPSA

    public int IdTipo { get; set; }

    public int IdEstado { get; set; }

    public string Observacion { get; set; } = String.Empty;

    public string Qr { get; set; } = String.Empty;

    public int Asistencia { get; set; }

    public int IdUbicacion { get; set; }

    public int IdUsuario { get; set; }

    public string Unidad { get; set; } = String.Empty;

    public string Coresponsable1 { get; set; } = String.Empty;

    public string Coresponsable2 { get; set; } = String.Empty;

    public string CargoCorres1 { get; set; } = String.Empty;

    public string CargoCorres2 { get; set; } = String.Empty;
}

public class EventCreateDto
{

    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public bool IsFeatured { get; set; }

    public string FeedImageUrl { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int IdTipo { get; set; }

    public int IdEstado { get; set; }

    public string Observacion { get; set; } = String.Empty;

    public string Qr { get; set; } = String.Empty;

    public int Asistencia { get; set; }

    public int IdUbicacion { get; set; }

    public int IdUsuario { get; set; }

    public string Unidad { get; set; } = String.Empty;

    public string Coresponsable1 { get; set; } = String.Empty;

    public string Coresponsable2 { get; set; } = String.Empty;

    public string CargoCorres1 { get; set; } = String.Empty;

    public string CargoCorres2 { get; set; } = String.Empty;
}



public class EventUpdateDto
{

    public int id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; } = String.Empty;

    public string? BannerImageUrl { get; set; } = String.Empty;

    public bool IsFeatured { get; set; }

    public string FeedImageUrl { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int IdTipo { get; set; }

    public int IdEstado { get; set; }

    public string Observacion { get; set; } = String.Empty;

    public string Qr { get; set; } = String.Empty;

    public int Asistencia { get; set; }

    public int IdUbicacion { get; set; }

    public int IdUsuario { get; set; }

    public string Unidad { get; set; } = String.Empty;

    public string Coresponsable1 { get; set; } = String.Empty;

    public string Coresponsable2 { get; set; } = String.Empty;

    public string CargoCorres1 { get; set; } = String.Empty;

    public string CargoCorres2 { get; set; } = String.Empty;
}



