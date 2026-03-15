namespace WebApiNibu.Data.Entity.FatherTable;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    [Required]
    public string CreatedBy { get; set; }
    
    public DateTime? CreatedAt { get; set; }

    public string UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [Required] public required bool Active { get; set; } = true;
}