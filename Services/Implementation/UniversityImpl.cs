using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class UniversityImpl(IBaseCrud<University> baseCrud, OracleDbContext db) : IUniversity
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<UniversityReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<UniversityReadDto>>.Success(dtos);
    }

    public async Task<Result<UniversityReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<UniversityReadDto>.Failure($"University with id {id} not found")
            : Result<UniversityReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<UniversityReadDto>>> GetFilteredAsync(UniversityFilter filter, CancellationToken ct)
    {
        var query = db.Universities.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<UniversityReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<UniversityReadDto>> CreateAsync(UniversityCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<UniversityReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, UniversityUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"University with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"University with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<University> ApplyFilters(IQueryable<University> query, UniversityFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Sigla))
            query = query.Where(x => x.Sigla != null && x.Sigla.Contains(filter.Sigla));

        if (!string.IsNullOrWhiteSpace(filter.Dpto))
            query = query.Where(x => x.Dpto != null && x.Dpto.Contains(filter.Dpto));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static UniversityReadDto MapToReadDto(University university) => new()
    {
        Id = university.Id,
        Name = university.Name,
        Sigla = university.Sigla ?? string.Empty,
        Dpto = university.Dpto ?? string.Empty,
        IdEventos = university.IdEventos ?? 0,
        OrdenEventos = university.OrdenEventos ?? 0,
        NivelCompetencia = university.NivelCompetencia ?? string.Empty
    };

    private static University MapFromCreateDto(UniversityCreateDto dto) => new()
    {
        Name = dto.Name,
        Sigla = dto.Sigla,
        Dpto = dto.Dpto,
        IdEventos = dto.IdEventos,
        OrdenEventos = dto.OrdenEventos,
        NivelCompetencia = dto.NivelCompetencia,
        Active = true
    };

    private static void ApplyUpdateDto(University target, UniversityUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Sigla = dto.Sigla;
        target.Dpto = dto.Dpto;
        target.IdEventos = dto.IdEventos;
        target.OrdenEventos = dto.OrdenEventos;
        target.NivelCompetencia = dto.NivelCompetencia;
    }
}
