using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Data.Entity.UsersAndAccess;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]

public class Users : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdUser { get; set; }

    [Required]
    [StringLength(12)]

    public required string Name { get; set; }

    [Required]
    [StringLength(12)]

    public required string Password { get; set; }

    [StringLength(450)]

    public string? ProfilePhoto { get; set; }

    [Required]

    public required PersonTable PersonTable { get; set; }

    [ForeignKey(nameof(PersonTable))]

    public int  IdPerson { get; set; } 

}