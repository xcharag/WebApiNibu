namespace WebApiNibu.Data.Entity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

public enum LevelInformation
{
    FourthGrade = 0,

    FifthGrade = 1,

    SixthGrade = 2

}

public enum WhatDepartment
{
    Beni = 0,

    Chuquisaca = 1,

    Cochabamba = 2,

    LaPaz = 3,

    Oruro = 4,

    Pando = 5,

    Potosi = 6,

    SantaCruz = 7,

    Tarija = 8


}

[Table("PreferencesStudent")]

public class PreferencesStudent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdPreferencesStudent { get; set; }

    [Required]

    public bool HaveVocationalTest { get; set; } = false;

    [Required]

    public LevelInformation LevelInformation { get; set; } = LevelInformation.FourthGrade;

    [Required]

    public bool StudyAbroad { get; set; } = false;

    [Required]

    public WhatDepartment WhatDepartment { get; set; } = WhatDepartment.SantaCruz;

}