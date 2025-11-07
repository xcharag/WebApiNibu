namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("StudentInterest")]

public class StudentInterest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdStudentInterest { get; set; }

    [Required]
    [StringLength(450)]

    public string Moment { get; set; } = string.Empty;

}