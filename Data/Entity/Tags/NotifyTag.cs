using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.Tags;

[Table("NotifyTag")]

public class NotifyTag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public required int Priority { get; set; }
    
    [Required]
    public required int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}