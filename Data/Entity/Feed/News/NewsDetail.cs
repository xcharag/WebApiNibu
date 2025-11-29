using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Enum;

namespace WebApiNibu.Data.Entity.Feed.News;

[Table("NewsDetail")]
public class NewsDetail : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] public int BlockNumber { get; set; } = 0;
    [Required] public NewsDetailType Type { get; set; } = NewsDetailType.Text;
    [Required] public string Content { get; set; } = string.Empty;
    [Required] public int NewsId { get; set; }
    public required News News { get; set; }
}

