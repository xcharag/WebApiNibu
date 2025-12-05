using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Entity.CopaUpsa;

public class Statistic : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(450)]
    public string? Description { get; set; } = string.Empty;
    
    public StatisticType StatisticType { get; set; }
    
    [Required]
    public int SportId { get; set; }
    public required Sport Sport { get; set; }
    
    public ICollection<StatisticEvent> StatisticEvents { get; set; } = new List<StatisticEvent>();
}