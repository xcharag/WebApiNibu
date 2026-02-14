using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Carreer;

public class CarreerQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<CarreerReadDto>>> GetAllAsync(
        CarreerFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.Carreers.AsQueryable();
        query = CarreerFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<CarreerReadDto>>.Success(new PagedResult<CarreerReadDto>
        {
            Items = items.Select(CarreerMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<CarreerReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Carreers.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<CarreerReadDto>.Failure($"Carreer with id {id} not found")
            : Result<CarreerReadDto>.Success(CarreerMapper.ToReadDto(item));
    }
}
