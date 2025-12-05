using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class StatisticEvent : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public TimeOnly Moment { get; set; }
    
    [Required]
    public int StatisticId { get; set; }
    public required Statistic Statistic { get; set; }
    
    [Required]
    public int RosterId { get; set; }
    public required Roster Roster { get; set; }
}