namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Person")]
public abstract class Person
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPersona { get; set; }

    [Required]
    [StringLength(50)]

    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string MiddleName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string PaternalSurname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string MaternalSurname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "datetime2")]

    public DateTime BirthDate { get; set; }

    [Required]
    [StringLength(30)]

    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]

    public string Email { get; set; } = string.Empty;

}