using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.Tags;

namespace WebApiNibu.Data.Entity.Feed.News;

[Table("News")]
public class News : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    public string? BannerImageUrl { get; set; } = string.Empty;
    public string? FeedImageUrl { get; set; } = string.Empty;
    
    public ICollection<NewsDetail> NewsDetails { get; set; } = new List<NewsDetail>();
    public ICollection<NewsReaction> NewsReactions { get; set; } = new List<NewsReaction>();
    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}