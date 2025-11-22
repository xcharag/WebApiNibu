namespace WebApiNibu.Data.Entity.UsersAndAccess;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Tag")]

public class Tag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdTag { get; set; }

    [Required]
    [StringLength(80)]

    public required string Name { get; set; }

    [Required]
    [StringLength(450)]

    public required string Icon { get; set; }

}
