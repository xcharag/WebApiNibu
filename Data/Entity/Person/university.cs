namespace WebApiNibu.Data.Entity.Person;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("University")]

public class University : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdUniversity { get; set; }

    [Required]
    [StringLength(450)]

    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]

    public string Sigla { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]

    public string Dpto { get; set; } = string.Empty;

    public int? IdEventos { get; set; }

    public int? OrdenEventos { get; set; }

    [Required]
    [StringLength(50)]

    public string NivelCompetencia { get; set; } = string.Empty;

    public ICollection<AcademicPreference>? AcademicPreferences { get; set; } = new List<AcademicPreference>();

    public ICollection<Carreer>? Carreers { get; set; } = new List<Carreer>();


}