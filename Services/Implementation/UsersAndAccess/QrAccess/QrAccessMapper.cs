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
        ExpirationDate = dto.ExpirationDate,
        Value = value,
        IsUsed = false,
        Active = true
    };

    public static Data.Entity.UsersAndAccess.QrAccess ToEntity(QrAccessGenerateDto dto, string generatedValue) => new()
    {
        Reason = dto.Reason,
        ExpirationDate = dto.ExpirationDate,
        Value = generatedValue,
        IsUsed = false,
        Active = true
    };

    public static void ApplyUpdate(Data.Entity.UsersAndAccess.QrAccess target, QrAccessUpdateDto dto)
    {
        target.Reason = dto.Reason;
        target.ExpirationDate = dto.ExpirationDate;
        target.Value = dto.Value;
        target.IsUsed = dto.IsUsed;
    }
}
