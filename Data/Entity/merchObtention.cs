namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MerchObtention")]
public class MerchObtention
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdMerchObtention { get; set; }

    [Required]
    [StringLength(450)]

    public string Reason { get; set; } = string.Empty;

    public SchoolStudent SchoolStudent { get; set; }

    [ForeignKey(nameof(SchoolStudent)]

    public int SchoolStudentId { get; set; }
}