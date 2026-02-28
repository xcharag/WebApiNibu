using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.UsersAndAccess;
using WebApiNibu.Data.Dto.UsersAndAccess.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.UsersAndAccess;
using WebApiNibu.Services.Implementation.UsersAndAccess.QrAccess;

namespace WebApiNibu.Services.Implementation.UsersAndAccess;

public class QrAccessImpl(IBaseCrud<Data.Entity.UsersAndAccess.QrAccess> baseCrud, CoreDbContext db)
    : IQrAccess
{
    private readonly QrAccessQueries _queries = new(db);
    private readonly QrAccessCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<QrAccessReadDto>>> GetAllAsync(QrAccessFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<QrAccessReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<QrAccessReadDto>> CreateAsync(QrAccessCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<QrAccessReadDto>> GenerateAsync(QrAccessGenerateDto dto, CancellationToken ct)
        => _commands.GenerateAsync(dto, ct);

    public Task<Result<QrAccessReadDto>> GenerateMarkUsedQrAsync(QrAccessGenerateDto dto, CancellationToken ct)
        => _commands.GenerateMarkUsedQrAsync(dto, ct);

    public Task<Result<QrMarkUsedResultDto>> ValidateAsync(QrValidateDto dto, CancellationToken ct)
        => _commands.ValidateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, QrAccessUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
