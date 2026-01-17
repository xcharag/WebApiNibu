using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.CopaUpsa;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Implementation;

public class SportImplementation(IBaseCrud<Sport> crud, OracleDbContext db) : ISport
{
    public async Task<IReadOnlyList<SportDto>> QueryAsync(SportQuery query, CancellationToken ct = default)
    {
        var q = db.Set<Sport>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            q = q.Where(s => s.Name.Contains(query.Name));

        if (query.Active.HasValue)
            q = q.Where(s => s.Active == query.Active.Value);

        var items = await q.ToListAsync(ct);
        return items.Select(MapToDto).ToList();
    }

    public async Task<SportDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = await crud.GetByIdAsync(id, true, ct);
        return item is null ? null : MapToDto(item);
    }

    public async Task<Result<SportDto>> CreateAsync(CreateSportCommand command, CancellationToken ct = default)
    {
        var dto = command.ToDto();
        var entity = MapFromCreateDto(dto);

        var created = await crud.CreateAsync(entity, ct);
        return Result<SportDto>.Ok(MapToDto(created));
    }

    public async Task<Result> UpdateAsync(int id, UpdateSportCommand command, CancellationToken ct = default)
    {
        var dto = command.ToDto();
        var updated = await crud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? Result.Ok() : Result.Fail("Not found", new[] { $"Sport (Id={id}) not found" });
    }

    public async Task<Result> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
    {
        var deleted = await crud.DeleteAsync(id, softDelete, ct);
        return deleted ? Result.Ok() : Result.Fail("Not found", new[] { $"Sport (Id={id}) not found" });
    }

    private static SportDto MapToDto(Sport s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Description = s.Description,
        Icon = s.Icon
    };

    private static Sport MapFromCreateDto(SportDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Icon = dto.Icon,
        Active = true
    };

    private static void ApplyUpdateDto(Sport target, SportDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.Icon = dto.Icon;
    }
}