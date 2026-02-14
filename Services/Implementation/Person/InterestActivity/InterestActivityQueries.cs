using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.InterestActivity;

public class InterestActivityQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<InterestActivitieReadDto>>> GetAllAsync(
        InterestActivityFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.InterestActivities.AsQueryable();
        query = InterestActivityFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<InterestActivitieReadDto>>.Success(new PagedResult<InterestActivitieReadDto>
        {
            Items = items.Select(InterestActivityMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<InterestActivitieReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.InterestActivities.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<InterestActivitieReadDto>.Failure($"InterestActivity with id {id} not found")
            : Result<InterestActivitieReadDto>.Success(InterestActivityMapper.ToReadDto(item));
    }
}
