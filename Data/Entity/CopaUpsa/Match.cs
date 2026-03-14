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
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal ScoreA { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal ScoreB { get; set; }
    
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
    public int ParticipationAId { get; set; }
    public required Participation ParticipationA { get; set; }
    
    [Required]
    public int ParticipationBId { get; set; }
    public required Participation ParticipationB { get; set; }
    
    [Required]
    public int MatchStatusId { get; set; }
    public required MatchStatus MatchStatus { get; set; }
    
    public ICollection<Roster> Rosters { get; set; } = new List<Roster>();
}