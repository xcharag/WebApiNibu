namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SchoolStudent")]
public class SchoolStudent : Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdSchoolStudent { get; set; }

    [Required]

    public int SchoolGrade { get; set; }

    [Required]

    public bool IsPlayer { get; set; } = false;

    public ICollection<Adult> Adults { get; set; } = new List<Adult>();

    public ICollection<PreferencesStudent> PreferencesStudents { get; set; } = new List<PreferencesStudent>();

    public ICollection? <MerchObtention> MerchObtentions { get; set; } = new List<MerchObtention>();

}