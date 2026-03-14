using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.UsersAndAccess;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.UsersAndAccess.QrAccess;

public class QrAccessCommands(IBaseCrud<Data.Entity.UsersAndAccess.QrAccess> baseCrud, CoreDbContext db)
{
    public async Task<Result<QrAccessReadDto>> CreateAsync(QrAccessCreateDto dto, CancellationToken ct)
    {
        var normalizedValue = string.IsNullOrWhiteSpace(dto.Value)
            ? Guid.NewGuid().ToString("N")
            : dto.Value.Trim();

        var validation = await ValidateAsync(
            dto.ExpirationDate,
            normalizedValue,
            dto.FirstName,
            dto.LastName,
            dto.DocumentNumber,
            dto.PhoneNumber,
            dto.Relationship,
            null,
            ct);
        if (!validation.IsSuccess)
            return Result<QrAccessReadDto>.Failure(validation.Errors);

        var entity = QrAccessMapper.ToEntity(dto, normalizedValue);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<QrAccessReadDto>.Success(QrAccessMapper.ToReadDto(created));
    }

    public async Task<Result<QrAccessReadDto>> GenerateAsync(QrAccessGenerateDto dto, CancellationToken ct)
    {
        var generatedValue = $"qr-{Guid.NewGuid():N}";

        var validation = await ValidateAsync(
            dto.ExpirationDate,
            generatedValue,
            dto.FirstName,
            dto.LastName,
            dto.DocumentNumber,
            dto.PhoneNumber,
            dto.Relationship,
            null,
            ct);
        if (!validation.IsSuccess)
            return Result<QrAccessReadDto>.Failure(validation.Errors);

        var entity = QrAccessMapper.ToEntity(dto, generatedValue);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<QrAccessReadDto>.Success(QrAccessMapper.ToReadDto(created));
    }

    public async Task<Result<QrAccessReadDto>> GenerateMarkUsedQrAsync(
        QrAccessGenerateDto dto,
        CancellationToken ct)
    {
        var hashedKey = await GenerateUniqueHashedKeyAsync(ct);
        var validation = await ValidateAsync(
            dto.ExpirationDate,
            hashedKey,
            dto.FirstName,
            dto.LastName,
            dto.DocumentNumber,
            dto.PhoneNumber,
            dto.Relationship,
            null,
            ct);
        if (!validation.IsSuccess)
            return Result<QrAccessReadDto>.Failure(validation.Errors);

        var entity = QrAccessMapper.ToEntity(dto, hashedKey);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<QrAccessReadDto>.Success(QrAccessMapper.ToReadDto(created));
    }

    public async Task<Result<QrMarkUsedResultDto>> ValidateAsync(QrValidateDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.HashedKey))
            return Result<QrMarkUsedResultDto>.Failure("HashedKey is required");

        var normalizedKey = dto.HashedKey.Trim();
        if (!IsSha256Hex(normalizedKey))
        {
            return Result<QrMarkUsedResultDto>.Success(new QrMarkUsedResultDto
            {
                MarkedAsUsed = false,
                AlreadyUsed = false,
                Expired = false,
                InvalidKey = true,
                Message = "Clave invalida para marcar el QR"
            });
        }

        var entity = await db.QrAccesses.FirstOrDefaultAsync(x => x.Value == normalizedKey && x.Active, ct);

        if (entity is null)
        {
            return Result<QrMarkUsedResultDto>.Success(new QrMarkUsedResultDto
            {
                MarkedAsUsed = false,
                AlreadyUsed = false,
                Expired = false,
                InvalidKey = true,
                Message = "Clave invalida para marcar el QR"
            });
        }

        if (entity.IsUsed)
        {
            return Result<QrMarkUsedResultDto>.Success(new QrMarkUsedResultDto
            {
                MarkedAsUsed = false,
                AlreadyUsed = true,
                Expired = false,
                InvalidKey = false,
                Message = "Qr ya fue utilizado"
            });
        }

        if (entity.ExpirationDate < DateTime.UtcNow)
        {
            return Result<QrMarkUsedResultDto>.Success(new QrMarkUsedResultDto
            {
                MarkedAsUsed = false,
                AlreadyUsed = false,
                Expired = true,
                InvalidKey = false,
                Message = "Qr expirado"
            });
        }

        entity.IsUsed = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result<QrMarkUsedResultDto>.Success(new QrMarkUsedResultDto
        {
            MarkedAsUsed = true,
            AlreadyUsed = false,
            Expired = false,
            InvalidKey = false,
            Message = "Qr escaneado correctamente"
        });
    }

    public async Task<Result<bool>> UpdateAsync(int id, QrAccessUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateAsync(
            dto.ExpirationDate,
            dto.Value.Trim(),
            dto.FirstName,
            dto.LastName,
            dto.DocumentNumber,
            dto.PhoneNumber,
            dto.Relationship,
            id,
            ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => QrAccessMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"QrAccess with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"QrAccess with id {id} not found");
    }

    private async Task<Result<bool>> ValidateAsync(
        DateTime expirationDate,
        string value,
        string firstName,
        string lastName,
        string documentNumber,
        string phoneNumber,
        string relationship,
        int? idToExclude,
        CancellationToken ct)
    {
        var errors = new List<string>();

        if (expirationDate <= DateTime.UtcNow)
            errors.Add("ExpirationDate must be greater than the current UTC date and time");

        if (string.IsNullOrWhiteSpace(value))
            errors.Add("Value is required");

        if (string.IsNullOrWhiteSpace(firstName))
            errors.Add("FirstName is required");

        if (string.IsNullOrWhiteSpace(lastName))
            errors.Add("LastName is required");

        if (string.IsNullOrWhiteSpace(documentNumber))
            errors.Add("DocumentNumber is required");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            errors.Add("PhoneNumber is required");

        if (string.IsNullOrWhiteSpace(relationship))
            errors.Add("Relationship is required");

        if (!string.IsNullOrWhiteSpace(value))
        {
            var exists = await db.QrAccesses.AnyAsync(
                x => x.Value == value && (!idToExclude.HasValue || x.Id != idToExclude.Value),
                ct);
            if (exists)
                errors.Add($"Value '{value}' is already registered");
        }

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    private async Task<string> GenerateUniqueHashedKeyAsync(CancellationToken ct)
    {
        while (true)
        {
            var rawKey = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var candidate = ComputeSha256Hex(rawKey);
            var exists = await db.QrAccesses.AnyAsync(x => x.Value == candidate, ct);
            if (!exists)
                return candidate;
        }
    }

    private static string ComputeSha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    private static bool IsSha256Hex(string value)
    {
        if (value.Length != 64) return false;
        foreach (var c in value)
        {
            var isDigit = c is >= '0' and <= '9';
            var isUpperHex = c is >= 'A' and <= 'F';
            var isLowerHex = c is >= 'a' and <= 'f';
            if (!isDigit && !isUpperHex && !isLowerHex) return false;
        }
        return true;
    }
}
