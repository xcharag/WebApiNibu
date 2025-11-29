using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.Feed.Events;

namespace WebApiNibu.Data.Entity.Tags;

public class EventTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int TagId { get; set; }
    public required Tag Tag { get; set; }
    
    [Required]
    public int EventId { get; set; }
    public required Event News { get; set; }
}