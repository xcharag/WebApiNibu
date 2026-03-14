using Net.Codecrete.QrCodeGenerator;
using WebApiNibu.Data.Dto.UsersAndAccess;

namespace WebApiNibu.Services.Implementation.UsersAndAccess.QrAccess;

public static class QrAccessMapper
{
    public static QrAccessReadDto ToReadDto(Data.Entity.UsersAndAccess.QrAccess entity)
    {
        var qr = QrCode.EncodeText(entity.Value, QrCode.Ecc.Medium);
        var png = qr.ToPngBitmap(border: 4, scale: 8);

        return new QrAccessReadDto
        {
            Id = entity.Id,
            Reason = entity.Reason,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DocumentNumber = entity.DocumentNumber,
            PhoneNumber = entity.PhoneNumber,
            Relationship = entity.Relationship,
            WasUpsaStudent = entity.WasUpsaStudent,
            ExpirationDate = entity.ExpirationDate,
            Value = entity.Value,
            IsUsed = entity.IsUsed,
            Active = entity.Active,
            QrSvg = qr.ToSvgString(4),
            QrPngBase64 = Convert.ToBase64String(png)
        };
    }

    public static Data.Entity.UsersAndAccess.QrAccess ToEntity(QrAccessCreateDto dto, string value) => new()
    {
        Reason = dto.Reason,
        FirstName = dto.FirstName.Trim(),
        LastName = dto.LastName.Trim(),
        DocumentNumber = dto.DocumentNumber.Trim(),
        PhoneNumber = dto.PhoneNumber.Trim(),
        Relationship = dto.Relationship.Trim(),
        WasUpsaStudent = dto.WasUpsaStudent,
        ExpirationDate = dto.ExpirationDate,
        Value = value,
        IsUsed = false,
        Active = true
    };

    public static Data.Entity.UsersAndAccess.QrAccess ToEntity(QrAccessGenerateDto dto, string generatedValue) => new()
    {
        Reason = dto.Reason,
        FirstName = dto.FirstName.Trim(),
        LastName = dto.LastName.Trim(),
        DocumentNumber = dto.DocumentNumber.Trim(),
        PhoneNumber = dto.PhoneNumber.Trim(),
        Relationship = dto.Relationship.Trim(),
        WasUpsaStudent = dto.WasUpsaStudent,
        ExpirationDate = dto.ExpirationDate,
        Value = generatedValue,
        IsUsed = false,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.UsersAndAccess.QrAccess target, QrAccessUpdateDto dto)
    {
        target.Reason = dto.Reason;
        target.FirstName = dto.FirstName.Trim();
        target.LastName = dto.LastName.Trim();
        target.DocumentNumber = dto.DocumentNumber.Trim();
        target.PhoneNumber = dto.PhoneNumber.Trim();
        target.Relationship = dto.Relationship.Trim();
        target.WasUpsaStudent = dto.WasUpsaStudent;
        target.ExpirationDate = dto.ExpirationDate;
        target.Value = dto.Value;
        target.IsUsed = dto.IsUsed;
    }
}
