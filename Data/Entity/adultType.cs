namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AdultType")]
public class AdultType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdAdultType { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;
}