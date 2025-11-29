using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.Tags;

[Table("Tag")]

public class Tag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public required string Name { get; set; }

    [Required]
    [StringLength(450)]
    public required string Icon { get; set; }

    [Required]
    public string Group { get; set; } = string.Empty;
    
    public ICollection<NotifyTag> NotifyTags { get; set; } = new List<NotifyTag>();
    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    public ICollection<PollTag> PollTags { get; set; } = new List<PollTag>();
    public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
}
