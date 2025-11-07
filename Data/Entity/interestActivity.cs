namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("InterestActivity")]

public class InterestActivity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdInterestActivity { get; set; }

    [Required]
    [StringLength(120)]

    public string Name { get; set; } = string.Empty;

    [StringLength(450)]

    public string? Description { get; set; }

    [Required]
    [StringLength(450)]

    public string Icon { get; set; } = string.Empty;
}