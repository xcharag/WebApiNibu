using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.Merch;

namespace WebApiNibu.Services.Implementation.Person;

public class MerchImpl : IMerch
{
    private readonly MerchQueries _queries;
    private readonly MerchCommands _commands;

    public MerchImpl(IBaseCrud<Data.Entity.Person.Merch> baseCrud, OracleDbContext db)
    {
        _queries = new MerchQueries(db);
        _commands = new MerchCommands(baseCrud, db);
    }

    // Queries
    public Task<Result<PagedResult<MerchReadDto>>> GetAllAsync(MerchFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<MerchReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<MerchReadDto>> CreateAsync(MerchCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, MerchUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
