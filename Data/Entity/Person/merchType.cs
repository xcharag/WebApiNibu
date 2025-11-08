namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

[Table("MerchType")]
public class MerchType : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdMerchType { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;

    [Required]

    public ICollection<Merch> Merches { get; set; } = new List<Merch>();

}