namespace WebApiNibu.Data.Entity.UsersAndAccess;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum InteractionType
{
    like = 0,

    favorite = 1
}

[Table("EventInteraction")]
public class EventInteraction : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdEventInteraction { get; set; }

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