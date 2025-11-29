using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.Feed.News;

namespace WebApiNibu.Data.Entity.Tags;

public class NewsTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int TagId { get; set; }
    public required Tag Tag { get; set; }
    
    [Required]
    public int NewsId { get; set; }
    public required News News { get; set; }
}