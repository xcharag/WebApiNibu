namespace WebApiNibu.Data.Dto.Feed.Polls;


public class OptionReadDto
{


    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public bool Correct { get; set; }

    //Foreign Keys

    public int PollId { get; set; }

    public int ParticipationId { get; set; }
}


public class OptionUpdateDto
{



    public string Name { get; set; } = String.Empty;

    public bool Correct { get; set; }

    //Foreign Keys

    public int PollId { get; set; }

    public int ParticipationId { get; set; }
}

public class OptionCreateDto
{



    public string Name { get; set; } = String.Empty;

    public bool Correct { get; set; }

    //Foreign Keys

    public int PollId { get; set; }

    public int ParticipationId { get; set; }
}
