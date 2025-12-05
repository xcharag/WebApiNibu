using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class MatchStatus : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(450)]
    public string? Description { get; set; }
    
    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;
    
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}