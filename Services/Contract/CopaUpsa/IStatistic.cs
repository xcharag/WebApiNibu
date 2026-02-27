using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IStatistic
{
    Task<Result<PagedResult<StatisticReadDto>>> GetAllAsync(StatisticFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<StatisticReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<StatisticReadDto>> CreateAsync(StatisticCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, StatisticUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

