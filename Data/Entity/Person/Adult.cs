namespace WebApiNibu.Data.Entity.Person;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Adult")]
public class Adult : PersonTable
{
    [Required]
    [StringLength(30)]

    public string WorkPhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]

    public string WorkEmail { get; set; } = string.Empty;

    [Required]
    public required SchoolStudent SchoolStudent { get; set; }

    [Required]
    public required AdultType AdultType { get; set; }

    [ForeignKey(nameof(SchoolStudent))]
    
    public int IdSchoolStudent { get; set; }

    [ForeignKey(nameof(AdultType))]

    public int IdAdultType { get; set; }
}