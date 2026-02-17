using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Polls;
using WebApiNibu.Services.Implementation.Feed.Polls.SelectedOption;

namespace WebApiNibu.Services.Implementation.Feed.Polls;

public class SelectedOptionImpl(IBaseCrud<Data.Entity.Feed.Polls.SelectedOption> baseCrud, CoreDbContext db)
    : ISelectedOption
{
    private readonly SelectedOptionQueries _queries = new(db);
    private readonly SelectedOptionCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<SelectedOptionReadDto>>> GetAllAsync(SelectedOptionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<SelectedOptionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<SelectedOptionReadDto>> CreateAsync(SelectedOptionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, SelectedOptionUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
