using WebApiNibu.Data.Dto.UsersAndAccess;
using WebApiNibu.Data.Dto.UsersAndAccess.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.UsersAndAccess;

public interface IQrAccess
{
    Task<Result<PagedResult<QrAccessReadDto>>> GetAllAsync(QrAccessFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<QrAccessReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<QrAccessReadDto>> CreateAsync(QrAccessCreateDto dto, CancellationToken ct);
    Task<Result<QrAccessReadDto>> GenerateAsync(QrAccessGenerateDto dto, CancellationToken ct);
    Task<Result<QrAccessReadDto>> GenerateMarkUsedQrAsync(QrAccessGenerateDto dto, CancellationToken ct);
    Task<Result<QrMarkUsedResultDto>> ValidateAsync(QrValidateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, QrAccessUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
