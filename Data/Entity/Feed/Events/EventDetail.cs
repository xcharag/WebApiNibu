using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Enum;

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

