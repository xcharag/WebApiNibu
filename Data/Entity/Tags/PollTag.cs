using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.Feed.Polls;

namespace WebApiNibu.Data.Entity.Tags;

public class PollTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int TagId { get; set; }
    public required Tag Tag { get; set; }
    
    [Required]
    public int PollId { get; set; }
    public required Poll Poll { get; set; }
}