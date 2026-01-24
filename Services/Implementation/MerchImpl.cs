using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class MerchImpl(IBaseCrud<Merch> baseCrud, OracleDbContext db) : IMerch
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<PagedResult<MerchReadDto>>> GetAllAsync(MerchFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Merchs.AsQueryable();
        query = ApplyFilters(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<MerchReadDto>>.Success(new PagedResult<MerchReadDto>
        {
            Items = items.Select(MapToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MerchReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<MerchReadDto>.Failure($"Merch with id {id} not found")
            : Result<MerchReadDto>.Success(MapToReadDto(item));
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<MerchReadDto>> CreateAsync(MerchCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.MerchTypeId, ct);
        if (!validationResult.IsSuccess)
            return Result<MerchReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.MerchTypeId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Merch with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Merch with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int merchTypeId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.MerchTypes.AnyAsync(mt => mt.Id == merchTypeId, ct))
            errors.Add($"MerchTypeId ({merchTypeId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Merch> ApplyFilters(IQueryable<Merch> query, MerchFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.MerchTypeId.HasValue)
            query = query.Where(x => x.IdMerchType == filter.MerchTypeId.Value);

        if (filter.Rarity.HasValue)
            query = query.Where(x => (int)x.Rarity == filter.Rarity.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static MerchReadDto MapToReadDto(Merch merch) => new()
    {
        Id = merch.Id,
        Name = merch.Name,
        Description = merch.Description,
        Icon = merch.Icon ?? string.Empty,
        Image = merch.Image ?? string.Empty,
        Rarity = (WebApiNibu.Data.Dto.Person.Rarity)merch.Rarity,
        MaxQuantity = merch.MaxQuantity,
        MerchTypeId = merch.IdMerchType
    };

    private static Merch MapFromCreateDto(MerchCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description ?? string.Empty,
        Icon = dto.Icon,
        Image = dto.Image,
        Rarity = (WebApiNibu.Data.Enum.Rarity)dto.Rarity,
        MaxQuantity = dto.MaxQuantity,
        IdMerchType = dto.MerchTypeId,
        Active = true,
        MerchType = null!
    };

    private static void ApplyUpdateDto(Merch target, MerchUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description ?? string.Empty;
        target.Icon = dto.Icon;
        target.Image = dto.Image;
        target.Rarity = (WebApiNibu.Data.Enum.Rarity)dto.Rarity;
        target.MaxQuantity = dto.MaxQuantity;
        target.IdMerchType = dto.MerchTypeId;
    }
}
