namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("School")]

public class School : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(150)]

    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]

    public string Tier { get; set; } = string.Empty;

    [Required]
    [StringLength(450)]

    public string Address { get; set; } = string.Empty;

    [StringLength(150)]

    public string? SportLogo { get; set; }

    [StringLength(150)]

    public string? NormalLogo { get; set; }

    public int Rue { get; set; }

    [Required]
    [StringLength(150)]

    public string Delegada { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]

    public string Tipo { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string Ciudad { get; set; } = string.Empty;

    public int IdDepartamento { get; set; }

    public int Ka { get; set; }

    public int IdDelegada { get; set; }

    public int IdColegio { get; set; }

    public int KaRectorada { get; set; }

    [StringLength(2)]

    public string? Segemento { get; set; }

    [Required]

    public ICollection<SchoolStudent> SchoolStudents { get; set; } = new List<SchoolStudent>();

    public ICollection<Contact>? Contacts { get; set; } = new List<Contact>();

}