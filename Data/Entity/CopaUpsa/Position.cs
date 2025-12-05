using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Position : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(5)]
    public string Code { get; set; } = string.Empty;
    
    [StringLength(120)]
    public string? Name { get; set; } = string.Empty;
    
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    
    ICollection<Roster> Rosters { get; set; } = new List<Roster>();
}