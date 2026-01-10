namespace WebApiNibu.Data.Dto;

public class SchoolTableReadDto
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? SportLogo { get; set; }
    public string? NormalLogo { get; set; }

    public int Rue { get; set; }
    public string Delegada { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    public int IdDepartamento { get; set; }
    public int Ka { get; set; }
    public int IdDelegada { get; set; }
    public int IdColegio { get; set; }
    public int KaRectorada { get; set; }
    public string? Segemento { get; set; }

    public List<ContactReadDto> Contacts { get; set; } = new();
}

public class SchoolTableCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? SportLogo { get; set; }
    public string? NormalLogo { get; set; }

    public int Rue { get; set; }
    public string Delegada { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    public int IdDepartamento { get; set; }
    public int Ka { get; set; }
    public int IdDelegada { get; set; }
    public int IdColegio { get; set; }
    public int KaRectorada { get; set; }
    public string? Segemento { get; set; }

    // Optional: create contacts alongside the school
    public List<ContactCreateDto> Contacts { get; set; } = new();
}

public class SchoolTableUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? SportLogo { get; set; }
    public string? NormalLogo { get; set; }

    public int Rue { get; set; }
    public string Delegada { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    public int IdDepartamento { get; set; }
    public int Ka { get; set; }
    public int IdDelegada { get; set; }
    public int IdColegio { get; set; }
    public int KaRectorada { get; set; }
    public string? Segemento { get; set; }
}

public class ContactReadDto
{
    public int Id { get; init; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonRole { get; set; } = string.Empty;
    public string? PersonPhoneNumber { get; set; }
    public string? PersonEmail { get; set; }
}

public class ContactCreateDto
{
    public string PersonName { get; set; } = string.Empty;
    public string PersonRole { get; set; } = string.Empty;
    public string? PersonPhoneNumber { get; set; }
    public string? PersonEmail { get; set; }
}

public class ContactUpdateDto
{
    public string PersonName { get; set; } = string.Empty;
    public string PersonRole { get; set; } = string.Empty;
    public string? PersonPhoneNumber { get; set; }
    public string? PersonEmail { get; set; }
}
