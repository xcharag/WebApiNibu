using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IRoster
{
    Task<Result<PagedResult<RosterReadDto>>> GetAllAsync(RosterFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<RosterReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<RosterReadDto>> CreateAsync(RosterCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, RosterUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

