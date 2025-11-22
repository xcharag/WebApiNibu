namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MerchType")]
public class MerchType : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;

    [Required]

    public ICollection<Merch> Merches { get; set; } = new List<Merch>();

}