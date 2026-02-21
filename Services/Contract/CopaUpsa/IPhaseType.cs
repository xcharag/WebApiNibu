using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IPhaseType
{
    Task<Result<PagedResult<PhaseTypeReadDto>>> GetAllAsync(PhaseTypeFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<PhaseTypeReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<PhaseTypeReadDto>> CreateAsync(PhaseTypeCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, PhaseTypeUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

