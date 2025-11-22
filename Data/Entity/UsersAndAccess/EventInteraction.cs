namespace WebApiNibu.Data.Entity.UsersAndAccess;

using FatherTable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum InteractionType
{
    Like = 0,
    Favorite = 1
}

[Table("EventInteraction")]
public class EventInteraction : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    public required InteractionType InteractionType { get; set; }

    [Required]
    [StringLength(200)]

    public required string NombreHermano { get; set; }

    [Required]
    [StringLength(1)]

    public required string Origen { get; set; }

    [Required]
    [StringLength(1)]

    public required string Estado { get; set; }

    [Required]

    public required int IdColegio { get; set; }

    [Required]

    public required int IdEstudiante { get; set; }


}