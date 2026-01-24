using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.MerchObtention;

namespace WebApiNibu.Services.Implementation.Person;

public class MerchObtentionImpl(IBaseCrud<Data.Entity.Person.MerchObtention> baseCrud, OracleDbContext db)
    : IMerchObtention
{
    private readonly MerchObtentionQueries _queries = new(db);
    private readonly MerchObtentionCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<MerchObtentionReadDto>>> GetAllAsync(MerchObtentionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
