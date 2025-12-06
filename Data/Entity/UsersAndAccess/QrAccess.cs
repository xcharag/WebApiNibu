namespace WebApiNibu.Data.Entity.UsersAndAccess;

using FatherTable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("QrAccess")]

public class QrAccess : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [StringLength(450)]

    public string? Reason { get; set; }

    [Required]
    public required DateTime ExpirationDate { get; set; }

    [Required]

    public required string Value { get; set; }

    [Required]
    
    public required bool IsUsed { get; set; }
}