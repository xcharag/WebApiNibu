using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.Feed.Polls;

[Table("Option")]
public class Option : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public bool Correct { get; set; } = false;
    
    [Required] public int PollId { get; set; }
    public required Poll Poll { get; set; }
    
    [Required] public int ParticipationId { get; set; }
    //TODO IMPORTANT: Add Participation Entity and relation with Option
    
    public ICollection<SelectedOption>? SelectedOptions { get; set; }
}