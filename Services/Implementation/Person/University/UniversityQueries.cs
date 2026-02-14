using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.University;

public class UniversityQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<UniversityReadDto>>> GetAllAsync(
        UniversityFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Universities.AsQueryable();
        query = UniversityFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<UniversityReadDto>>.Success(new PagedResult<UniversityReadDto>
        {
            Items = items.Select(UniversityMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<UniversityReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Universities.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<UniversityReadDto>.Failure($"University with id {id} not found")
            : Result<UniversityReadDto>.Success(UniversityMapper.ToReadDto(item));
    }
}
