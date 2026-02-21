using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface ITournament
{
    Task<Result<PagedResult<TournamentReadDto>>> GetAllAsync(TournamentFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<TournamentReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<TournamentReadDto>> CreateAsync(TournamentCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, TournamentUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

