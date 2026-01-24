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

    public async Task<Result<PagedResult<MerchObtentionReadDto>>> GetAllAsync(MerchObtentionFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.MerchObtentions.AsQueryable();
        query = ApplyFilters(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);
        return Result<PagedResult<MerchObtentionReadDto>>.Success(new PagedResult<MerchObtentionReadDto>
        {
            Items = items.Select(MapToReadDto), PageNumber = pagination.PageNumber, PageSize = pagination.PageSize, TotalCount = totalCount
        });
    }

    public async Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<MerchObtentionReadDto>.Failure($"MerchObtention with id {id} not found")
            : Result<MerchObtentionReadDto>.Success(MapToReadDto(item));
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validation.IsSuccess) return Result<MerchObtentionReadDto>.Failure(validation.Errors);
        var created = await baseCrud.CreateAsync(MapFromCreateDto(dto), ct);
        return Result<MerchObtentionReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validation.IsSuccess) return Result<bool>.Failure(validation.Errors);
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, int merchId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct)) errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        if (!await db.Merchs.AnyAsync(m => m.Id == merchId, ct)) errors.Add($"MerchId ({merchId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<MerchObtention> ApplyFilters(IQueryable<MerchObtention> query, MerchObtentionFilter filter)
    {
        if (filter.SchoolStudentId.HasValue) query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);
        if (filter.MerchId.HasValue) query = query.Where(x => x.IdMerch == filter.MerchId.Value);
        if (filter.Claimed.HasValue) query = query.Where(x => x.Claimed == filter.Claimed.Value);
        if (filter.Active.HasValue) query = query.Where(x => x.Active == filter.Active.Value);
        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static MerchObtentionReadDto MapToReadDto(MerchObtention o) => new() { Id = o.Id, Reason = o.Reason, Claimed = o.Claimed, SchoolStudentId = o.IdSchoolStudent, MerchId = o.IdMerch };
    private static MerchObtention MapFromCreateDto(MerchObtentionCreateDto dto) => new() { Reason = dto.Reason ?? string.Empty, Claimed = dto.Claimed, IdSchoolStudent = dto.SchoolStudentId, IdMerch = dto.MerchId, Active = true, SchoolStudent = null!, Merch = null! };
    private static void ApplyUpdateDto(MerchObtention t, MerchObtentionCreateDto dto) { t.Reason = dto.Reason ?? string.Empty; t.Claimed = dto.Claimed; t.IdSchoolStudent = dto.SchoolStudentId; t.IdMerch = dto.MerchId; }
}
