namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DocumentType")]

public class DocumentType : BaseEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

	public int IdDocumentType { get; set; }

	[Required]
	[StringLength(20)]

	public string Name { get; set; } = string.Empty;

	public ICollection<Person> People { get; set; } = new List<Person>();

}