using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Entity.Feed.News;

[Table("NewsReaction")]

public class NewsReaction : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required] public int NewsId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int MerchId { get; set; }
    
    public required News News { get; set; }
    public required Users User { get; set; }
    public required Merch Merch { get; set; }
}