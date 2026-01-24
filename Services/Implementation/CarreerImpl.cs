using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class CarreerImpl(IBaseCrud<Carreer> baseCrud, OracleDbContext db) : ICarreer
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<CarreerReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<CarreerReadDto>>.Success(dtos);
    }

    public async Task<Result<CarreerReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<CarreerReadDto>.Failure($"Carreer with id {id} not found")
            : Result<CarreerReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<CarreerReadDto>>> GetFilteredAsync(CarreerFilter filter, CancellationToken ct)
    {
        var query = db.Carreers.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<CarreerReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<CarreerReadDto>> CreateAsync(CarreerCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<CarreerReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CarreerUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Carreer with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Carreer with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<Carreer> ApplyFilters(IQueryable<Carreer> query, CarreerFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.AreaFormacionId.HasValue)
            query = query.Where(x => x.AreaFormacionId == filter.AreaFormacionId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static CarreerReadDto MapToReadDto(Carreer carreer) => new()
    {
        Id = carreer.Id,
        Name = carreer.Name,
        BannerImage = carreer.BannerImage,
        Description = carreer.Description,
        AreaFormacionId = carreer.AreaFormacionId,
        CodCarrRel = carreer.CodCarrRel,
        OrdenEventos = carreer.OrdenEventos
    };

    private static Carreer MapFromCreateDto(CarreerCreateDto dto) => new()
    {
        Name = dto.Name,
        BannerImage = dto.BannerImage ?? string.Empty,
        Description = dto.Description,
        AreaFormacionId = dto.AreaFormacionId,
        CodCarrRel = dto.CodCarrRel,
        OrdenEventos = dto.OrdenEventos,
        Active = true
    };

    private static void ApplyUpdateDto(Carreer target, CarreerUpdateDto dto)
    {
        target.Name = dto.Name;
        target.BannerImage = dto.BannerImage ?? string.Empty;
        target.Description = dto.Description;
        target.AreaFormacionId = dto.AreaFormacionId;
        target.CodCarrRel = dto.CodCarrRel;
        target.OrdenEventos = dto.OrdenEventos;
    }
}