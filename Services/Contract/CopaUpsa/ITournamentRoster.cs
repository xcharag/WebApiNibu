using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface ITournamentRoster
{
    Task<Result<PagedResult<TournamentRosterReadDto>>> GetAllAsync(TournamentRosterFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<TournamentRosterReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<TournamentRosterReadDto>> CreateAsync(TournamentRosterCreateDto dto, CancellationToken ct);
    Task<Result<TournamentRosterUploadResultDto>> UploadFromExcel(IFormFile file, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, TournamentRosterUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

