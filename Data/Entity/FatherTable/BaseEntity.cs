namespace WebApiNibu.Data.Entity.FatherTable;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BaseEntity")]

public abstract class BaseEntity
{
    [Required]
    public required int CreatedBy { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public required DateTime CreatedAt { get; set; }

    [Required]
    public required int UpdatedBy { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public required DateTime UpdatedAt { get; set; }

    [Required]
    public required bool Active { get; set; }

}