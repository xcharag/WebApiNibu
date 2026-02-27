using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IMatchStatus
{
    Task<Result<PagedResult<MatchStatusReadDto>>> GetAllAsync(MatchStatusFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<MatchStatusReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<MatchStatusReadDto>> CreateAsync(MatchStatusCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MatchStatusUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

