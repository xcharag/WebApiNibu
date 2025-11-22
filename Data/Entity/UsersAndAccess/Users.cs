

namespace WebApiNibu.Data.Entity.UsersAndAccess;

using FatherTable;
using Person;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]

public class Users : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

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