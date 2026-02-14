using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.SelectedOption;

public class SelectedOptionCommands(IBaseCrud<Data.Entity.Feed.Polls.SelectedOption> baseCrud, OracleDbContext db)
{
    public async Task<Result<SelectedOptionReadDto>> CreateAsync(SelectedOptionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.OptionId, dto.UserId, ct);
        if (!validationResult.IsSuccess)
            return Result<SelectedOptionReadDto>.Failure(validationResult.Errors);

        var entity = SelectedOptionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<SelectedOptionReadDto>.Success(SelectedOptionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, SelectedOptionUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.OptionId, dto.UserId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => SelectedOptionMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"SelectedOption with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"SelectedOption with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int optionId, int userId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Options.AnyAsync(o => o.Id == optionId, ct))
            errors.Add($"OptionId ({optionId}) not found");

        if (!await db.Users.AnyAsync(u => u.Id == userId, ct))
            errors.Add($"UserId ({userId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}
