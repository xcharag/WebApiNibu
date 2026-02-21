using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.CopaUpsa;

public interface IParticipation
{
    Task<Result<PagedResult<ParticipationReadDto>>> GetAllAsync(ParticipationFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<ParticipationReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<ParticipationReadDto>> CreateAsync(ParticipationCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, ParticipationUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

