using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.SelectedOption;

public class SelectedOptionQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<SelectedOptionReadDto>>> GetAllAsync(
        SelectedOptionFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        var query = db.SelectedOptions.AsQueryable();
        query = SelectedOptionFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<SelectedOptionReadDto>>.Success(new PagedResult<SelectedOptionReadDto>
        {
            Items = items.Select(SelectedOptionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<SelectedOptionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.SelectedOptions.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<SelectedOptionReadDto>.Failure($"SelectedOption with id {id} not found")
            : Result<SelectedOptionReadDto>.Success(SelectedOptionMapper.ToReadDto(item));
    }
}
