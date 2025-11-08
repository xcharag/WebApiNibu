namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AdultType")]
public class AdultType : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdAdultType { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;

    [Required]
    public ICollection<Adult> Adults { get; set; } = new List<Adult>();
}