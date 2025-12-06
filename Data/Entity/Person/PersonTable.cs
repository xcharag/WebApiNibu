namespace WebApiNibu.Data.Entity.Person;

using FatherTable;
using WebApiNibu.Data.Entity.UsersAndAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("personTable")]
public abstract class PersonTable : BaseEntity
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]

    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]

    public string? MiddleName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string PaternalSurname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string MaternalSurname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]

    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [StringLength(30)]

    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]

    public string Email { get; set; } = string.Empty;

    [Required]
    public required Country Country { get; set; }

    [Required]
    public required DocumentType DoucmentType { get; set; }

    public Users? User { get; set; }

    [ForeignKey(nameof(Country))]

    public int IdCountry { get; set; }

    [ForeignKey(nameof(DoucmentType))]

    public int IdDocumentType { get; set; }

}