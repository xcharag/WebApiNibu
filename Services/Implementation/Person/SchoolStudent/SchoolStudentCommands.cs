using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.SchoolStudent;

public class SchoolStudentCommands(IBaseCrud<Data.Entity.Person.SchoolStudent> baseCrud, CoreDbContext db)
{
    public async Task<Result<SchoolStudentReadDto>> CreateAsync(SchoolStudentCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (!validation.IsSuccess)
            return Result<SchoolStudentReadDto>.Failure(validation.Errors);

        var entity = SchoolStudentMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<SchoolStudentReadDto>.Success(SchoolStudentMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, SchoolStudentUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => SchoolStudentMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"SchoolStudent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"SchoolStudent with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int idCountry, int idDocumentType, int idSchool, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Countries.AnyAsync(c => c.Id == idCountry, ct))
            errors.Add($"IdCountry ({idCountry}) not found");

        if (!await db.DocumentTypes.AnyAsync(d => d.Id == idDocumentType, ct))
            errors.Add($"IdDocumentType ({idDocumentType}) not found");

        if (!await db.Schools.AnyAsync(s => s.Id == idSchool, ct))
            errors.Add($"IdSchool ({idSchool}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}
