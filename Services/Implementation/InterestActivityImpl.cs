using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class InterestActivityImpl(IBaseCrud<InterestActivity> baseCrud, OracleDbContext db) : IInterestActivity
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<InterestActivitieReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<InterestActivitieReadDto>>.Success(dtos);
    }

    public async Task<Result<InterestActivitieReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<InterestActivitieReadDto>.Failure($"InterestActivity with id {id} not found")
            : Result<InterestActivitieReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<InterestActivitieReadDto>>> GetFilteredAsync(InterestActivityFilter filter, CancellationToken ct)
    {
        var query = db.InterestActivities.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<InterestActivitieReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<InterestActivitieReadDto>> CreateAsync(InterestActivitieCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<InterestActivitieReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, InterestActivitieUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"InterestActivity with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"InterestActivity with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<InterestActivity> ApplyFilters(IQueryable<InterestActivity> query, InterestActivityFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static InterestActivitieReadDto MapToReadDto(InterestActivity activity) => new()
    {
        Id = activity.Id,
        Name = activity.Name,
        Description = activity.Description,
        Icon = activity.Icon
    };

    private static InterestActivity MapFromCreateDto(InterestActivitieCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Icon = dto.Icon,
        Active = true
    };

    private static void ApplyUpdateDto(InterestActivity target, InterestActivitieUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.Icon = dto.Icon;
    }
}
