using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class AdultImpl(IBaseCrud<Adult> baseCrud, OracleDbContext db) : IAdult
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<AdultReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AdultReadDto>>.Success(dtos);
    }

    public async Task<Result<AdultReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<AdultReadDto>.Failure($"Adult with id {id} not found")
            : Result<AdultReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<AdultReadDto>>> GetFilteredAsync(AdultFilter filter, CancellationToken ct)
    {
        var query = db.Adults.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<AdultReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<AdultReadDto>> CreateAsync(AdultCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<AdultReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AdultReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AdultUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Adult with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Adult with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int adultTypeId, int schoolStudentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.AdultTypes.AnyAsync(a => a.Id == adultTypeId, ct))
            errors.Add($"AdultTypeId ({adultTypeId}) not found");

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Adult> ApplyFilters(IQueryable<Adult> query, AdultFilter filter)
    {
        if (filter.AdultTypeId.HasValue)
            query = query.Where(x => x.IdAdultType == filter.AdultTypeId.Value);

        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (!string.IsNullOrWhiteSpace(filter.WorkEmail))
            query = query.Where(x => x.WorkEmail.Contains(filter.WorkEmail));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static AdultReadDto MapToReadDto(Adult adult) => new()
    {
        Id = adult.Id,
        WorkPhoneNumber = adult.WorkPhoneNumber,
        WorkEmail = adult.WorkEmail,
        AdultTypeId = adult.IdAdultType,
        SchoolStudentId = adult.IdSchoolStudent
    };

    private static Adult MapFromCreateDto(AdultCreateDto dto) => new()
    {
        WorkPhoneNumber = dto.WorkPhoneNumber,
        WorkEmail = dto.WorkEmail,
        IdAdultType = dto.AdultTypeId,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        AdultType = null!,
        SchoolStudent = null!
    };

    private static void ApplyUpdateDto(Adult target, AdultUpdateDto dto)
    {
        target.WorkPhoneNumber = dto.WorkPhoneNumber;
        target.WorkEmail = dto.WorkEmail;
        target.IdAdultType = dto.AdultTypeId;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}