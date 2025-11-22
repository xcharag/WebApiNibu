using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Entity.Feed.Events;

[Table("EventInteraction")]
public class EventInteraction : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    
    [Required]
    public bool IsAttending { get; set; } = false;
    
    [Required]
    public int EventId { get; set; }
    public required Event Event { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public required Users User { get; set; }
    
    public int? QrId { get; set; }
    public QrAccess? QrAccess { get; set; }
    
    public int? MerchId { get; set; }
    public Merch? Merch { get; set; }
    
    //INTEGRACION
    [StringLength(200)]
    public string? NombreHermano { get; set; } = string.Empty;
    [StringLength(1)]
    public string? Origen { get; set; } = string.Empty;
    [StringLength(1)]
    public string? Estado { get; set; } = string.Empty;
    public int IdColegio { get; set; } = 0;
    public int IdEstudiante { get; set; } = 0;
}