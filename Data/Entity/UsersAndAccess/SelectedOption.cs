namespace WebApiNibu.Data.Entity.UsersAndAccess;

using FatherTable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SelectedOption")]

public class SelectedOption : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

}