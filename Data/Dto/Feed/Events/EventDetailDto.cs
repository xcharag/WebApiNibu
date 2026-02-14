namespace WebApiNibu.Data.Dto.Feed.Events;

public enum EventDetailType
{

}

public class EventDetailReadDto
{

    public int Id { get; set; }

    public int BlockNumber { get; set; }

    public EventDetailType EventDetailType { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys

    public int EventId { get; set; }
}

public class EventDetailUpdateDto
{


    public int BlockNumber { get; set; }

    public EventDetailType EventDetailType { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys

    public int EventId { get; set; }
}

public class EventDetailCreateDto
{

    public int Id { get; set; }

    public int BlockNumber { get; set; }

    public EventDetailType EventDetailType { get; set; }

    public string Content { get; set; } = String.Empty;

    //Foreign Keys

    public int EventId { get; set; }
}
