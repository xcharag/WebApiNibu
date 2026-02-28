using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.UsersAndAccess;
using WebApiNibu.Data.Dto.UsersAndAccess.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.UsersAndAccess.QrAccess;

public class QrAccessQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<QrAccessReadDto>>> GetAllAsync(
        QrAccessFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.QrAccesses.AsQueryable();
        query = QrAccessFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<QrAccessReadDto>>.Success(new PagedResult<QrAccessReadDto>
        {
            Items = items.Select(QrAccessMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<QrAccessReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.QrAccesses.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<QrAccessReadDto>.Failure($"QrAccess with id {id} not found")
            : Result<QrAccessReadDto>.Success(QrAccessMapper.ToReadDto(item));
    }
}
