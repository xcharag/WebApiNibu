using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class PhaseType : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public required string Name { get; set; } = string.Empty;
    
    [StringLength(450)]
    public string? Description { get; set; } = string.Empty;
    
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
}