using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.News;
using WebApiNibu.Services.Implementation.Feed.News.News;

namespace WebApiNibu.Services.Implementation.Feed.News;

public class NewsImpl(IBaseCrud<Data.Entity.Feed.News.News> baseCrud, CoreDbContext db)
    : INews
{
    private readonly NewsQueries _queries = new(db);
    private readonly NewsCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<NewsReadDto>>> GetAllAsync(NewsFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<NewsReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<NewsReadDto>> CreateAsync(NewsCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, NewsUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
