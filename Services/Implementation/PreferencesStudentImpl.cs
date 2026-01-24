using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class PreferencesStudentImpl(IBaseCrud<PreferencesStudent> baseCrud, OracleDbContext db) : IPreferencesStudent
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<PreferencesStudentReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<PreferencesStudentReadDto>>.Success(dtos);
    }

    public async Task<Result<PreferencesStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<PreferencesStudentReadDto>.Failure($"PreferencesStudent with id {id} not found")
            : Result<PreferencesStudentReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<PreferencesStudentReadDto>>> GetFilteredAsync(PreferencesStudentFilter filter, CancellationToken ct)
    {
        var query = db.PreferencesStudents.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<PreferencesStudentReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<PreferencesStudentReadDto>> CreateAsync(PreferencesStudentCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<PreferencesStudentReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<PreferencesStudentReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, PreferencesStudentUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"PreferencesStudent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"PreferencesStudent with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<PreferencesStudent> ApplyFilters(IQueryable<PreferencesStudent> query, PreferencesStudentFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.HaveVocationalTest.HasValue)
            query = query.Where(x => x.HaveVocationalTest == filter.HaveVocationalTest.Value);

        if (filter.StudyAbroad.HasValue)
            query = query.Where(x => x.StudyAbroad == filter.StudyAbroad.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static PreferencesStudentReadDto MapToReadDto(PreferencesStudent preferences) => new()
    {
        Id = preferences.Id,
        HaveVocationalTest = preferences.HaveVocationalTest,
        StudyAbroad = preferences.StudyAbroad,
        WhereHadTest = (WebApiNibu.Data.Dto.Person.WhereHadTest?)preferences.WhereHadTest,
        LevelInformation = (WebApiNibu.Data.Dto.Person.LevelInformation?)preferences.LevelInformation,
        SchoolStudentId = preferences.IdSchoolStudent
    };

    private static PreferencesStudent MapFromCreateDto(PreferencesStudentCreateDto dto) => new()
    {
        HaveVocationalTest = dto.HaveVocationalTest,
        StudyAbroad = dto.StudyAbroad,
        WhereHadTest = dto.WhereHadTest.HasValue
            ? (WebApiNibu.Data.Enum.WhereHadTest)dto.WhereHadTest.Value
            : WebApiNibu.Data.Enum.WhereHadTest.School,
        LevelInformation = dto.LevelInformation.HasValue
            ? (WebApiNibu.Data.Enum.LevelInformation)dto.LevelInformation.Value
            : WebApiNibu.Data.Enum.LevelInformation.Little,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        SchoolStudent = null!
    };

    private static void ApplyUpdateDto(PreferencesStudent target, PreferencesStudentUpdateDto dto)
    {
        target.HaveVocationalTest = dto.HaveVocationalTest;
        target.StudyAbroad = dto.StudyAbroad;
        target.WhereHadTest = dto.WhereHadTest.HasValue
            ? (WebApiNibu.Data.Enum.WhereHadTest)dto.WhereHadTest.Value
            : target.WhereHadTest;
        target.LevelInformation = dto.LevelInformation.HasValue
            ? (WebApiNibu.Data.Enum.LevelInformation)dto.LevelInformation.Value
            : target.LevelInformation;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}
