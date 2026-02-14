using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public class PollQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<PollReadDto>>> GetAllAsync(
        PollFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.Polls.AsQueryable();
        query = PollFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<PollReadDto>>.Success(new PagedResult<PollReadDto>
        {
            Items = items.Select(PollMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<PollReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Polls.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PollReadDto>.Failure($"Poll with id {id} not found")
            : Result<PollReadDto>.Success(PollMapper.ToReadDto(item));
    }
}
