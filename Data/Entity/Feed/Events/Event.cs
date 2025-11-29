using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Tags;

namespace WebApiNibu.Data.Entity.Feed.Events;

[Table("Event")]
public class Event : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450)]
    public string? Description { get; set; } = string.Empty;
    
    [StringLength(450)]
    public string? BannerImageUrl { get; set; } = string.Empty;
    
    public bool IsFeatured { get; set; }
    
    [StringLength(450)]
    public string? FeedImageUrl { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    
    public ICollection<EventInteraction> EventInteractions { get; set; } = new List<EventInteraction>();
    public ICollection<EventDetail> EventDetails { get; set; } = new List<EventDetail>();
    public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    
    //INTEGRACION
    public int IdTipo { get; set; } = 0;
    public int IdEstado { get; set; } = 0;
    public string? Observacion { get; set; } = string.Empty;
    public string? Qr { get; set; } = string.Empty;
    public int Asistencia { get; set; } = 0;
    public int IdUbicacion { get; set; } = 0;
    public int IdUsuario { get; set; } = 0;
    public string Unidad { get; set; } = string.Empty;
    public string? Corresponsable1 { get; set; } = string.Empty;
    public string? Corresponsable2 { get; set; } = string.Empty;
    public string? Cargores { get; set; } = string.Empty;
    public string? CargoCorres1 { get; set; } = string.Empty;
    public string? CargoCorres2 { get; set; } = string.Empty;
}