using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Roster : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int MatchId { get; set; }
    public required Match Match { get; set; }
    
    [Required]
    public int SchoolStudentId { get; set; }
    public required SchoolStudent SchoolStudent { get; set; }
    
    [Required]
    public int PositionId { get; set; }
    public required Position Position { get; set; }
    
    public ICollection<StatisticEvent> StatisticEvents { get; set; } = new List<StatisticEvent>();
}