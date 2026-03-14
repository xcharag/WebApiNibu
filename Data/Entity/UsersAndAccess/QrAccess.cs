namespace WebApiNibu.Data.Entity.UsersAndAccess;

using FatherTable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.School;

[Table("QrAccess")]

public class QrAccess : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [StringLength(450)]

    public string? Reason { get; set; }

    [StringLength(150)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(150)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(50)]
    public string DocumentNumber { get; set; } = string.Empty;

    [StringLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(100)]
    public string Relationship { get; set; } = string.Empty;

    [Required]
    public bool WasUpsaStudent { get; set; }

    [Required]
    public required DateTime ExpirationDate { get; set; }

    [Required]

    public required string Value { get; set; }

    [Required]

    public required bool IsUsed { get; set; }

    [StringLength(450)]
    public string? Comment { get; set; }

    public int? SchoolTableId { get; set; }
    public SchoolTable? SchoolTable { get; set; }
}
