using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PreferencesStudent")]

public class PreferencesStudent : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    public bool HaveVocationalTest { get; set; } = false;
    [Required]
    public WhereHadTest WhereHadTest { get; set; } = WhereHadTest.School;

    [Required]
    public LevelInformation LevelInformation { get; set; } = LevelInformation.Little;

    [Required]
    public bool StudyAbroad { get; set; } = false;
    
    [Required]
    public required SchoolStudent SchoolStudent { get; set; }

    public ICollection<AcademicPreference> AcademicPreferences { get; set; } = new List<AcademicPreference>();

    [ForeignKey(nameof(SchoolStudent))]
    public int IdSchoolStudent { get; set; }    
}