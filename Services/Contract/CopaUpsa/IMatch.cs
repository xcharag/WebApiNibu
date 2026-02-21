using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IMatch
{
    Task<Result<PagedResult<MatchReadDto>>> GetAllAsync(MatchFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<MatchReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<MatchReadDto>> CreateAsync(MatchCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MatchUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

