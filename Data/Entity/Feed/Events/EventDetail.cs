using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.Feed.Events;

[Table("EventDetail")]
public class EventDetail : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int BlockNumber { get; set; } = 0;
    [Required] public EventDetailType Type { get; set; } = EventDetailType.Text;
    [Required] public string Content { get; set; } = string.Empty;
    [Required] public int EventId { get; set; }
    public required Event Event { get; set; }
}

public enum EventDetailType
{
    Text = 0,
    ImageNoCap = 1,
    ImageWithCap = 2,
    VideoNoCap = 3,
    VideoWithCap = 4,
    AudioNoCap = 5,
    AudioWithCap = 6,
    File = 7
}