namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Country")]

public class Country : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdCountry { get; set; }

    [Required]
    [StringLength(20)]

    public string name { get; set; } = string.Empty;

    public ICollection<Person> People { get; set; } = new List<Person>();
       

}