namespace WebApiNibu.Data.Entity.UsersAndAccess;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("QrAccess")]

public class QrAccess : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdQrAccess { get; set; }

    [StringLength(450)]

    public string? Reason { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]

    public required DateTime ExpirationDate { get; set; }

    [Required]

    public required string Value { get; set; }

    [Required]
    
    public required bool IsUsed { get; set; }
}