namespace WebApiNibu.Data.Entity.Person;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Worker")]
public class Worker : PersonTable
{
    [Required]
    [StringLength(120)]
    public string WorkEmail { set; get; } = string.Empty;

    [Required]
    public required Role Role { set; get; }

    [ForeignKey(nameof(Role))]
    public int IdRole { set; get; } 
    

}