namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MerchObtention")]
public class MerchObtention : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Reason { get; set; } = string.Empty;

    [Required] public bool Claimed { get; set; } = false;

    [Required]
    public required SchoolStudent SchoolStudent { get; set; }

    [Required]

    public required Merch Merch { get; set; }

    [ForeignKey(nameof(SchoolStudent))]

    public int IdSchoolStudent { get; set; }

    [ForeignKey(nameof(Merch))]

    public int IdMerch { get; set; }

}