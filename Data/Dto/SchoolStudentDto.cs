namespace WebApiNibu.Data.Dto;

// DTO returned to consumers
public class SchoolStudentReadDto
{
    public int Id { get; set; }
    // Person basic info
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string PaternalSurname { get; set; } = string.Empty;
    public string MaternalSurname { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Foreign keys
    public int IdCountry { get; set; }
    public int IdDocumentType { get; set; }

    // School student specifics
    public int IdSchool { get; set; }
    public int SchoolGrade { get; set; }
    public bool IsPlayer { get; set; }
    public bool HasUpsaParents { get; set; }
}

// DTO used to create a new SchoolStudent
public class SchoolStudentCreateDto
{
    // Person basic info
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string PaternalSurname { get; set; } = string.Empty;
    public string MaternalSurname { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Foreign keys
    public int IdCountry { get; set; }
    public int IdDocumentType { get; set; }

    // School student specifics
    public int IdSchool { get; set; }
    public int SchoolGrade { get; set; }
    public bool IsPlayer { get; set; }
    public bool HasUpsaParents { get; set; }
}

// DTO used to update an existing SchoolStudent
public class SchoolStudentUpdateDto
{
    // Person basic info
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string PaternalSurname { get; set; } = string.Empty;
    public string MaternalSurname { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Foreign keys
    public int IdCountry { get; set; }
    public int IdDocumentType { get; set; }

    // School student specifics
    public int IdSchool { get; set; }
    public int SchoolGrade { get; set; }
    public bool IsPlayer { get; set; }
    public bool HasUpsaParents { get; set; }
}
