namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Adult")]
public class Adult : Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdAdult { get; set; }

    [Required]
    [StringLength(30)]

    public string WorkPhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]

    public string WorkEmail { get; set; } = string.Empty;
}