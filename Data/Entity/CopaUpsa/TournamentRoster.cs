using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.School;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class TournamentRoster : BaseEntity
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int SchoolStudentId { get; set; }
    public required SchoolStudent SchoolStudent { get; set; }
    
    [Required]
    public int TournamentId { get; set; }
    public required Tournament Tournament { get; set; }
    
    [Required]
    public int SchoolId { get; set; }
    public required SchoolTable SchoolTable { get; set; }
    
    public ICollection<Roster> Rosters { get; set; } = new List<Roster>();
}