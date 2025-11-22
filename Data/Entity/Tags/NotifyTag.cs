using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNibu.Data.Entity.FatherTable;
using WebApiNibu.Data.Entity.UsersAndAccess;

namespace WebApiNibu.Data.Entity.Tags;

[Table("NotifyTag")]

public class NotifyTag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public required int Priority { get; set; }

    [Required] public NotifyType NotifyType { get; set; } = NotifyType.None;
    
    [Required]
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
    
    [Required]
    public int UserId { get; set; }
    public required Users User { get; set; }
}

public enum NotifyType
{
    Email = 0,
    Push = 1,
    WhatsApp = 2,
    Sms = 3,
    All = 4,
    None = 5
}