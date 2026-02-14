using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Worker;

public class WorkerQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<WorkerReadDto>>> GetAllAsync(
        WorkerFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Workers.AsQueryable();
        query = WorkerFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<WorkerReadDto>>.Success(new PagedResult<WorkerReadDto>
        {
            Items = items.Select(WorkerMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<WorkerReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Workers.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<WorkerReadDto>.Failure($"Worker with id {id} not found")
            : Result<WorkerReadDto>.Success(WorkerMapper.ToReadDto(item));
    }
}
