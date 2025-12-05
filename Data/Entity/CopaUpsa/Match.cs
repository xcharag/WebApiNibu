using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Match : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ScoreA { get; set; } = 0m;
    
    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ScoreB { get; set; } = 0m;
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal DetailPointA { get; set; } = 0m;
    [Column(TypeName = "decimal(18,4)")]
    public decimal DetailPointB { get; set; } = 0m;
    
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int NumberMatch { get; set; } = 0;
    
    [Required]
    public int ParticipationId { get; set; }
    public required Participation Participation { get; set; }
    
    [Required]
    public int MatchStatusId { get; set; }
    public required MatchStatus MatchStatus { get; set; }
    
    public ICollection<Roster> Rosters { get; set; } = new List<Roster>();
}