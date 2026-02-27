using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface ITournamentParent
{
    Task<Result<PagedResult<TournamentParentReadDto>>> GetAllAsync(TournamentParentFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<TournamentParentReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<TournamentParentReadDto>> CreateAsync(TournamentParentCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, TournamentParentUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

