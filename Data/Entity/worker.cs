namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Worker")]
public class Worker : Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WokerID { set; get; }

    [Required]
    [StringLength(120)]

    public string WorkEmail { set; get; } = string.Empty;
}