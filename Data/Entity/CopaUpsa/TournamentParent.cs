using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class TournamentParent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [StringLength(450)]
    public string? Description { get; set; } = string.Empty;
    public Category Category { get; set; }
}