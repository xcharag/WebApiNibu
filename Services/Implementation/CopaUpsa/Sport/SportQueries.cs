using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Sport;

public class SportQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<SportReadDto>>> GetAllAsync(
        SportFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Sports.AsQueryable();
        query = SportFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<SportReadDto>>.Success(new PagedResult<SportReadDto>
        {
            Items = items.Select(SportMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<SportReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Sports.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<SportReadDto>.Failure($"Sport with id {id} not found")
            : Result<SportReadDto>.Success(SportMapper.ToReadDto(item));
    }
}

