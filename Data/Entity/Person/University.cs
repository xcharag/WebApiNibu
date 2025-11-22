namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("University")]

public class University : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<AcademicPreference>? AcademicPreferences { get; set; } = new List<AcademicPreference>();
    public ICollection<Carreer>? Carreers { get; set; } = new List<Carreer>();
    
    // INTEGRATION WITH EVENT SYSTEM
    [StringLength(150)]
    public string? Sigla { get; set; } = string.Empty;
    
    [StringLength(30)]
    public string? Dpto { get; set; } = string.Empty;

    public int? IdEventos { get; set; }
    public int? OrdenEventos { get; set; }
    
    [StringLength(50)]
    public string? NivelCompetencia { get; set; } = string.Empty;
}