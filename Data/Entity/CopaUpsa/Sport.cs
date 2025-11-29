using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Sport : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450)]
    public string? Description { get; set; } = string.Empty;
    
    public string Icon { get; set; } = string.Empty;
    
    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}