using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface ISport
{
    // Queries
    Task<Result<PagedResult<SportReadDto>>> GetAllAsync(SportFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<SportReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<SportReadDto>> CreateAsync(SportCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, SportUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

