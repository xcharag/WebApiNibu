using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class AcademicPreferenceImpl(IBaseCrud<AcademicPreference> baseCrud, OracleDbContext db) : IAcademicPreference
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<AcademicPreferenceReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AcademicPreferenceReadDto>>.Success(dtos);
    }

    public async Task<Result<AcademicPreferenceReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<AcademicPreferenceReadDto>.Failure($"AcademicPreference with id {id} not found")
            : Result<AcademicPreferenceReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<AcademicPreferenceReadDto>>> GetFilteredAsync(AcademicPreferenceFilter filter, CancellationToken ct)
    {
        var query = db.AcademicPreferences.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AcademicPreferenceReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<AcademicPreferenceReadDto>> CreateAsync(AcademicPreferenceCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<AcademicPreferenceReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AcademicPreferenceReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AcademicPreferenceUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
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

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<AcademicPreference> ApplyFilters(IQueryable<AcademicPreference> query, AcademicPreferenceFilter filter)
    {
        if (filter.UniversityId.HasValue)
            query = query.Where(x => x.IdUniversitiy == filter.UniversityId.Value);

        if (filter.CarreerId.HasValue)
            query = query.Where(x => x.IdCarreer == filter.CarreerId.Value);

        if (filter.PreferencesStudentId.HasValue)
            query = query.Where(x => x.IdPreferencesStudent == filter.PreferencesStudentId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static AcademicPreferenceReadDto MapToReadDto(AcademicPreference entity) => new()
    {
        Id = entity.Id,
        UniversityId = entity.IdUniversitiy,
        CarreerId = entity.IdCarreer,
        PreferencesStudentId = entity.IdPreferencesStudent
    };

    private static AcademicPreference MapFromCreateDto(AcademicPreferenceCreateDto dto) => new()
    {
        IdUniversitiy = dto.UniversityId ?? 0,
        IdCarreer = dto.CarreerId ?? 0,
        IdPreferencesStudent = dto.PreferencesStudentId,
        Active = true,
        PreferencesStudent = null!,
        Universitiy = null!,
        Carreer = null!
    };

    private static void ApplyUpdateDto(AcademicPreference target, AcademicPreferenceUpdateDto dto)
    {
        target.IdUniversitiy = dto.UniversityId ?? target.IdUniversitiy;
        target.IdCarreer = dto.CarreerId ?? target.IdCarreer;
        target.IdPreferencesStudent = dto.PreferencesStudentId;
    }
}