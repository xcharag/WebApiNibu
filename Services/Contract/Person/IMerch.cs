using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IMerch
{
    // Queries
    Task<Result<PagedResult<MerchReadDto>>> GetAllAsync(MerchFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<MerchReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<MerchReadDto>> CreateAsync(MerchCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MerchUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
