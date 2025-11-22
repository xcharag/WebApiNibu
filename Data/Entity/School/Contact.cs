using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;

namespace WebApiNibu.Data.Entity.School;

[Table("Contact")]

public class Contact : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

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