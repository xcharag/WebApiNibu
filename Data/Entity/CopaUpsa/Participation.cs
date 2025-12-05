using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Feed.Polls;
using WebApiNibu.Data.Entity.School;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Participation : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(2)]
    public string Key { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    [Required]
    public int PhaseTypeId { get; set; }
    public required PhaseType PhaseType { get; set;}
    
    [Required]
    public int TournamentId { get; set; }
    public required Tournament Tournament { get; set; }
    
    [Required]
    public int SchoolId { get; set; }
    public required SchoolTable SchoolTable { get; set; }
    
    ICollection<Match> Matches { get; set; } = new List<Match>();
    ICollection<Option> Options { get; set; } = new List<Option>();
}