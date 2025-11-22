namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Role")]
public class Role : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(80)]

    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string Department { get; set; } = string.Empty;

    public ICollection<Worker> Workers { get; set; } = new List<Worker>();
}