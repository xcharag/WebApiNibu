using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Polls;
using WebApiNibu.Services.Implementation.Feed.Polls.Option;

namespace WebApiNibu.Services.Implementation.Feed.Polls;

public class OptionImpl(IBaseCrud<Data.Entity.Feed.Polls.Option> baseCrud, CoreDbContext db)
    : IOption
{
    private readonly OptionQueries _queries = new(db);
    private readonly OptionCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<OptionReadDto>>> GetAllAsync(OptionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<OptionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<OptionReadDto>> CreateAsync(OptionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, OptionUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
