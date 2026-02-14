using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.MerchType;

namespace WebApiNibu.Services.Implementation.Person;

public class MerchTypeImpl(IBaseCrud<Data.Entity.Person.MerchType> baseCrud, CoreDbContext db)
    : IMerchType
{
    private readonly MerchTypeQueries _queries = new(db);
    private readonly MerchTypeCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<MerchTypeReadDto>>> GetAllAsync(MerchTypeFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<MerchTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<MerchTypeReadDto>> CreateAsync(MerchTypeCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, MerchTypeUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
