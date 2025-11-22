namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Carreer")]

public class Carreer : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(450)]

    public string BannerImage { get; set; } = string.Empty;

    [StringLength(500)]

    public string? Description { get; set; }
    
    public int AreaFormacionId { get; set; }

    public int CodCarrRel { get; set; }

    public int OrdenEventos { get; set; }

    public University? University { get ; set;}  

    public ICollection<AcademicPreference>? AcademicPreferences { get; set; } = new List<AcademicPreference>();
}
