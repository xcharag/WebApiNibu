using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Option;

public class OptionQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<OptionReadDto>>> GetAllAsync(
        OptionFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.Options.AsQueryable();
        query = OptionFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<OptionReadDto>>.Success(new PagedResult<OptionReadDto>
        {
            Items = items.Select(OptionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<OptionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Options.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<OptionReadDto>.Failure($"Option with id {id} not found")
            : Result<OptionReadDto>.Success(OptionMapper.ToReadDto(item));
    }
}
