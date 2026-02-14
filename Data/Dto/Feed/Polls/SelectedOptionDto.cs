namespace WebApiNibu.Data.Dto.Feed.Polls;

public class SelectedOptionReadDto
{


    public int Id { get; set; }

    //Foreign Keys 

    public int OptionId { get; set; }

    public int UserId { get; set; }

}


public class SelectedOptionUpdateDto
{



    //Foreign Keys 

    public int OptionId { get; set; }

    public int UserId { get; set; }
}

public class SelectedOptionCreateDto
{



    //Foreign Keys 

    public int OptionId { get; set; }

    public int UserId { get; set; }
}
