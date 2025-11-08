namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AcademicPreference")]

public class AcademicPreference : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdAcademicPreference { get; set; }
    
    [Required]
    public required PreferencesStudent PreferencesStudent { get; set; }

    [Required]
    public required University Universitiy { get; set; }

    [Required]
    public required Carreer Carreer { get; set; }


    [ForeignKey(nameof(PreferencesStudent))]

    public int IdPreferencesStudent { get; set; }


    [ForeignKey(nameof(Universitiy))]

    public int IdUniversitiy { get; set; }


    [ForeignKey(nameof(Carreer))]

    public int IdCarreer { get; set; }
}
