using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class MerchObtentionImpl(IBaseCrud<MerchObtention> baseCrud, OracleDbContext db) : IMerchObtention
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<MerchObtentionReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<MerchObtentionReadDto>>.Success(dtos);
    }

    public async Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<MerchObtentionReadDto>.Failure($"MerchObtention with id {id} not found")
            : Result<MerchObtentionReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<MerchObtentionReadDto>>> GetFilteredAsync(MerchObtentionFilter filter, CancellationToken ct)
    {
        var query = db.MerchObtentions.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<MerchObtentionReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<MerchObtentionReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchObtentionReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, int merchId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        if (!await db.Merchs.AnyAsync(m => m.Id == merchId, ct))
            errors.Add($"MerchId ({merchId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<MerchObtention> ApplyFilters(IQueryable<MerchObtention> query, MerchObtentionFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.MerchId.HasValue)
            query = query.Where(x => x.IdMerch == filter.MerchId.Value);

        if (filter.Claimed.HasValue)
            query = query.Where(x => x.Claimed == filter.Claimed.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static MerchObtentionReadDto MapToReadDto(MerchObtention obtention) => new()
    {
        Id = obtention.Id,
        Reason = obtention.Reason,
        Claimed = obtention.Claimed,
        SchoolStudentId = obtention.IdSchoolStudent,
        MerchId = obtention.IdMerch
    };

    private static MerchObtention MapFromCreateDto(MerchObtentionCreateDto dto) => new()
    {
        Reason = dto.Reason ?? string.Empty,
        Claimed = dto.Claimed,
        IdSchoolStudent = dto.SchoolStudentId,
        IdMerch = dto.MerchId,
        Active = true,
        SchoolStudent = null!,
        Merch = null!
    };

    private static void ApplyUpdateDto(MerchObtention target, MerchObtentionCreateDto dto)
    {
        target.Reason = dto.Reason ?? string.Empty;
        target.Claimed = dto.Claimed;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdMerch = dto.MerchId;
    }
}
