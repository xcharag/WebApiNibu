using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.News;
using WebApiNibu.Services.Implementation.Feed.News.NewsDetail;

namespace WebApiNibu.Services.Implementation.Feed.News;

public class NewsDetailImpl(IBaseCrud<Data.Entity.Feed.News.NewsDetail> baseCrud, OracleDbContext db)
    : INewsDetail
{
    private readonly NewsDetailQueries _queries = new(db);
    private readonly NewsDetailCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<NewsDetailReadDto>>> GetAllAsync(NewsDetailFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<NewsDetailReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<NewsDetailReadDto>> CreateAsync(NewsDetailCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, NewsDetailUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
