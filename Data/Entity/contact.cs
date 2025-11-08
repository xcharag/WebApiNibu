namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Contact")]

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdContact { get; set; }

    [Required]
    [StringLength(150)]

    public string PersonName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]

    public string PersonRole { get; set; } = string.Empty;

    [StringLength(30)]

    public string? PersonPhoneNumber { get; set; }

    [StringLength(30)]

    public string? PersonEmail { get; set; }

    [Required]

    public required School School { get; set; }

}