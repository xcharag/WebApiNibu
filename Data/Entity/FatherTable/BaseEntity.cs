namespace WebApiNibu.Data.Entity.FatherTable;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    public int CreatedBy { get; set; } = 0;
    
    public DateTime? CreatedAt { get; set; }

    public int UpdatedBy { get; set; } = 0;
    public DateTime? UpdatedAt { get; set; }

    [Required] public required bool Active { get; set; } = true;
}