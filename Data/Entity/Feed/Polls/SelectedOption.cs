using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Entity.Feed.Polls;

[Table("SelectedOption")]

public class SelectedOption : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required] public int OptionId { get; set; }
    public required Option Option { get; set; }
    
    [Required] public int UserId { get; set; }
    public required Users User { get; set; }
}