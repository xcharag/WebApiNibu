using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class WorkerImpl(IBaseCrud<Worker> baseCrud, OracleDbContext db) : IWorker
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<WorkerReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<WorkerReadDto>>.Success(dtos);
    }

    public async Task<Result<WorkerReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<WorkerReadDto>.Failure($"Worker with id {id} not found")
            : Result<WorkerReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<WorkerReadDto>>> GetFilteredAsync(WorkerFilter filter, CancellationToken ct)
    {
        var query = db.Workers.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<WorkerReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<WorkerReadDto>> CreateAsync(WorkerCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.RoleId, ct);
        if (!validationResult.IsSuccess)
            return Result<WorkerReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<WorkerReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, WorkerUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.RoleId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Worker with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Worker with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int roleId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Roles.AnyAsync(r => r.Id == roleId, ct))
            errors.Add($"RoleId ({roleId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Worker> ApplyFilters(IQueryable<Worker> query, WorkerFilter filter)
    {
        if (filter.RoleId.HasValue)
            query = query.Where(x => x.IdRole == filter.RoleId.Value);

        if (!string.IsNullOrWhiteSpace(filter.WorkEmail))
            query = query.Where(x => x.WorkEmail.Contains(filter.WorkEmail));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static WorkerReadDto MapToReadDto(Worker worker) => new()
    {
        Id = worker.Id,
        WorkEmail = worker.WorkEmail,
        RoleId = worker.IdRole
    };

    private static Worker MapFromCreateDto(WorkerCreateDto dto) => new()
    {
        WorkEmail = dto.WorkEmail,
        IdRole = dto.RoleId,
        Active = true,
        Role = null!,
        Country = null!,
        DoucmentType = null!
    };

    private static void ApplyUpdateDto(Worker target, WorkerUpdateDto dto)
    {
        target.WorkEmail = dto.WorkEmail;
        target.IdRole = dto.RoleId;
    }
}
