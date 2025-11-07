namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MerchType")]
public class MerchType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdMerchType { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;

}