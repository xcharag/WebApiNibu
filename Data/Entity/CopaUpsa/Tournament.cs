using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Tournament : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450)]
    public string? Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    
    [StringLength(450)]
    public string? Logo { get; set; }
    [StringLength(450)]
    public string? Icon { get; set; }
    [StringLength(450)]
    public string? Banner { get; set; }

    public bool HasGroupStage { get; set; } = true;
    public bool HasPlayOffStage { get; set; } = true;
    
    [Required]
    public int TournamentParentId { get; set; }
    public required TournamentParent TournamentParent { get; set; }
    
    [Required]
    public int SportId { get; set; }
    public required Sport Sport { get; set; }
    
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
}