namespace WebApiNibu.Data.Entity.FatherTable;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    [Required]
    public required int CreatedBy { get; set; }

    [Required]
    public required DateTime CreatedAt { get; set; }

    [Required]
    public required int UpdatedBy { get; set; }

    [Required]
    public required DateTime UpdatedAt { get; set; }

    [Required]
    public required bool Active { get; set; }
}