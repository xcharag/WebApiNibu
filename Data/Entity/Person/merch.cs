namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum Rarity
{
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}

[Table("Merch")]
public class Merch : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdMerch { get; set; }

    [Required]
    [StringLength(120)]

    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(450)]

    public string Description { get; set; } = string.Empty;

    [StringLength(450)]

    public string? Icon { get; set; }

    [StringLength(450)]

    public string? Image { get; set; }

    [Required]

    public Rarity Rarity { get; set; } = Rarity.Common;

    [Required]
    public int MaxQuantity { get; set; }

    public ICollection<MerchObtention>? MerchObtentions { get; set; } = new List<MerchObtention>();

    [Required]

    public required MerchType MerchType { get; set; }

    [Required]

    public required int IdMerchType { get; set; }

}