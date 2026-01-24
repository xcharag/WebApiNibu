using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class MerchTypeImpl(IBaseCrud<MerchType> baseCrud, OracleDbContext db) : IMerchType
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<MerchTypeReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<MerchTypeReadDto>>.Success(dtos);
    }

    public async Task<Result<MerchTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<MerchTypeReadDto>.Failure($"MerchType with id {id} not found")
            : Result<MerchTypeReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<MerchTypeReadDto>>> GetFilteredAsync(MerchTypeFilter filter, CancellationToken ct)
    {
        var query = db.MerchTypes.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<MerchTypeReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<MerchTypeReadDto>> CreateAsync(MerchTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchTypeReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MerchType with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<MerchType> ApplyFilters(IQueryable<MerchType> query, MerchTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static MerchTypeReadDto MapToReadDto(MerchType merchType) => new()
    {
        Id = merchType.Id,
        Name = merchType.Name
    };

    private static MerchType MapFromCreateDto(MerchTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(MerchType target, MerchTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}
