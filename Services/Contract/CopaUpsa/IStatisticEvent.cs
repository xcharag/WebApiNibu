using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IStatisticEvent
{
    Task<Result<PagedResult<StatisticEventReadDto>>> GetAllAsync(StatisticEventFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<StatisticEventReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<StatisticEventReadDto>> CreateAsync(StatisticEventCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, StatisticEventUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

