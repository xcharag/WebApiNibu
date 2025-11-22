namespace WebApiNibu.Data.Entity.Person;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SchoolStudent")]
public class SchoolStudent : PersonTable
{
    [Required]
    public int SchoolGrade { get; set; }

    [Required]
    public bool IsPlayer { get; set; } = false;

    [Required]
    public ICollection<Adult> Adults { get; set; } = new List<Adult>();

    [Required]
    public ICollection<PreferencesStudent> PreferencesStudents { get; set; } = new List<PreferencesStudent>();

    public ICollection<MerchObtention>? MerchObtentions { get; set; } = new List<MerchObtention>();

    [Required]
    public ICollection<StudentInterest> StudentInterests { get; set; } = new List<StudentInterest>();

    [Required]
    public required School School { get; set; }

    [Required]
    [ForeignKey(nameof(School))]
    public required int IdSchool { get; set; }


}