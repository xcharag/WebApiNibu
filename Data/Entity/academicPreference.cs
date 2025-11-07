namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AcademicPreference")]

public class AcademicPreference
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdAcademicPreference { get; set; }

    public PreferencesStudent PreferencesStudent { get; set; }

    [ForeignKey(nameof(PreferencesStudent))]

    public int PreferencesStudentId { get; set; }
}