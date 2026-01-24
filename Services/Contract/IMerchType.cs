using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IMerchType
{
    // Queries
    Task<Result<PagedResult<MerchTypeReadDto>>> GetAllAsync(MerchTypeFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<MerchTypeReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<MerchTypeReadDto>> CreateAsync(MerchTypeCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MerchTypeUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
