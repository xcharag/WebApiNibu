namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MerchObtention")]
public class MerchObtention : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdMerchObtention { get; set; }

    [Required]
    [StringLength(450)]

    public string Reason { get; set; } = string.Empty;

    [Required]
    public required SchoolStudent SchoolStudent { get; set; }

    [Required]

    public required Merch Merch { get; set; }

    [ForeignKey(nameof(SchoolStudent))]

    public int IdSchoolStudent { get; set; }

    [ForeignKey(nameof(Merch))]

    public int IdMerch { get; set; }

}