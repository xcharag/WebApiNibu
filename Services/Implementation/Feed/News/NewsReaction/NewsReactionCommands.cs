using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

public class NewsReactionCommands(IBaseCrud<Data.Entity.Feed.News.NewsReaction> baseCrud, OracleDbContext db)
{
    public async Task<Result<NewsReactionReadDto>> CreateAsync(NewsReactionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.NewsId, dto.UserId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<NewsReactionReadDto>.Failure(validationResult.Errors);

        var entity = NewsReactionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<NewsReactionReadDto>.Success(NewsReactionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, NewsReactionUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.NewsId, dto.UserId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => NewsReactionMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"NewsReaction with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"NewsReaction with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int newsId, int userId, int merchId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.News.AnyAsync(n => n.Id == newsId, ct))
            errors.Add($"NewsId ({newsId}) not found");

        if (!await db.Users.AnyAsync(u => u.Id == userId, ct))
            errors.Add($"UserId ({userId}) not found");

        if (!await db.Merchs.AnyAsync(m => m.Id == merchId, ct))
            errors.Add($"MerchId ({merchId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}
