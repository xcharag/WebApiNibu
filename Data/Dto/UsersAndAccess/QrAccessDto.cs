namespace WebApiNibu.Data.Dto.UsersAndAccess;

public class QrAccessReadDto
{
    public int Id { get; set; }
    public string? Reason { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool Active { get; set; }
    public string QrSvg { get; set; } = string.Empty;
    public string QrPngBase64 { get; set; } = string.Empty;
}

public class QrAccessCreateDto
{
    public string? Reason { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Value { get; set; }
}

public class QrAccessGenerateDto
{
    public string? Reason { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class QrAccessUpdateDto
{
    public string? Reason { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
}

public class QrMarkUsedResultDto
{
    public bool MarkedAsUsed { get; set; }
    public bool AlreadyUsed { get; set; }
    public bool Expired { get; set; }
    public bool InvalidKey { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class QrValidateDto
{
    public string HashedKey { get; set; } = string.Empty;
}
