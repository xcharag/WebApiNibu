using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IPosition
{
    Task<Result<PagedResult<PositionReadDto>>> GetAllAsync(PositionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<PositionReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<PositionReadDto>> CreateAsync(PositionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, PositionUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

