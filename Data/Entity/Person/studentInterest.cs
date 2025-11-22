namespace WebApiNibu.Data.Entity.Person;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("StudentInterest")]

public class StudentInterest : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdStudentInterest { get; set; }

    [Required]
    [StringLength(450)]

    public string? Moment { get; set; } = string.Empty;

    [Required]

    public required SchoolStudent SchoolStudent { get; set; }

    [Required]

    public required InterestActivity InterestActivity { get; set; }

    [Required]
    [ForeignKey(nameof(SchoolStudent))]

    public required int IdSchoolStudent { get; set; }

    [Required]
    [ForeignKey(nameof(InterestActivity))]

    public required int IdInterestActivity { get; set; }
}