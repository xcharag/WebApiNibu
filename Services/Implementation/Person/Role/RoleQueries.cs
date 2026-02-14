using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Role;

public class RoleQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<RoleReadDto>>> GetAllAsync(
        RoleFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Roles.AsQueryable();
        query = RoleFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<RoleReadDto>>.Success(new PagedResult<RoleReadDto>
        {
            Items = items.Select(RoleMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<RoleReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Roles.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<RoleReadDto>.Failure($"Role with id {id} not found")
            : Result<RoleReadDto>.Success(RoleMapper.ToReadDto(item));
    }
}
