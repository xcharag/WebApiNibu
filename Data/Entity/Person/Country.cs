namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Country")]

public class Country : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(20)]

    public string Name { get; set; } = string.Empty;

    public ICollection<PersonTable> People { get; set; } = new List<PersonTable>();
       

}