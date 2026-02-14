namespace WebApiNibu.Data.Dto.Feed.Events;

public class EventInteractionReadDto
{


    public int Id { get; set; }

    //Foreign Keys

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int QrAccessId { get; set; }

    public int MerchId { get; set; }

    /////////////////////////

    public bool isAttending { get; set; }

    /////////////////////////

    //Integracion con sistema UPSA

    public string NombreHermano { get; set; } = String.Empty;

    public string Origen { get; set; } = String.Empty;

    public int IdColegio { get; set; }

    public int IdEstudiante { get; set; }
}


public class EventInteractionCreateDto
{

    //Foreign Keys

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int QrAccessId { get; set; }

    public int MerchId { get; set; }

    /////////////////////////

    public bool isAttending { get; set; }

    /////////////////////////

    //Integracion con sistema UPSA

    public string NombreHermano { get; set; } = String.Empty;

    public string Origen { get; set; } = String.Empty;

    public int IdColegio { get; set; }

    public int IdEstudiante { get; set; }
}



public class EventInteractionUpdateDto
{



    //Foreign Keys

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int QrAccessId { get; set; }

    public int MerchId { get; set; }

    /////////////////////////

    public bool isAttending { get; set; }

    /////////////////////////

    //Integracion con sistema UPSA

    public string NombreHermano { get; set; } = String.Empty;

    public string Origen { get; set; } = String.Empty;

    public int IdColegio { get; set; }

    public int IdEstudiante { get; set; }
}
