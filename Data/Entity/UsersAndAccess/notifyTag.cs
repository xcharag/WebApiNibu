namespace WebApiNibu.Data.Entity.UsersAndAccess;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("NotifyTag")]

public class NotifyTag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdNotifyTag { get; set; }

    [Required]

    public required int Priority { get; set; }

}