using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.AcademicPreference;

public class AcademicPreferenceCommands(IBaseCrud<Data.Entity.Person.AcademicPreference> baseCrud, CoreDbContext db)
{
    public async Task<Result<AcademicPreferenceReadDto>> CreateAsync(AcademicPreferenceCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<AcademicPreferenceReadDto>.Failure(validationResult.Errors);

        var entity = AcademicPreferenceMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AcademicPreferenceReadDto>.Success(AcademicPreferenceMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AcademicPreferenceUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => AcademicPreferenceMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AcademicPreference with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AcademicPreference with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int? universityId, int? carreerId, int preferencesStudentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (universityId is null)
            errors.Add("UniversityId is required");
        else if (!await db.Universities.AnyAsync(u => u.Id == universityId, ct))
            errors.Add($"UniversityId ({universityId}) not found");

        if (carreerId is null)
            errors.Add("CarreerId is required");
        else if (!await db.Carreers.AnyAsync(c => c.Id == carreerId, ct))
            errors.Add($"CarreerId ({carreerId}) not found");

        if (!await db.PreferencesStudents.AnyAsync(p => p.Id == preferencesStudentId, ct))
            errors.Add($"PreferencesStudentId ({preferencesStudentId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}
