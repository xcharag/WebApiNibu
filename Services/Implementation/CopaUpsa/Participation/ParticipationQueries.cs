using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Participation;

public class ParticipationQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<ParticipationReadDto>>> GetAllAsync(
        ParticipationFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Participations.AsQueryable();
        query = ParticipationFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<ParticipationReadDto>>.Success(new PagedResult<ParticipationReadDto>
        {
            Items = items.Select(ParticipationMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<ParticipationReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Participations.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<ParticipationReadDto>.Failure($"Participation with id {id} not found")
            : Result<ParticipationReadDto>.Success(ParticipationMapper.ToReadDto(item));
    }
}

