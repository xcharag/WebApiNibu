using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Tags;

namespace WebApiNibu.Data.Entity.Feed.Polls;

[Table("Poll")]
public class Poll : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Required] public string Question { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    
    [Required]
    public int TournamentId { get; set; }
    //TODO IMPLEMENT TOURNAMENT ENTITY RELATION
    
    public ICollection<Option> Options { get; set; } = new List<Option>();
    public ICollection<PollTag>? PollTags { get; set; } = new List<PollTag>();
}