using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Option;

public class OptionCommands(IBaseCrud<Data.Entity.Feed.Polls.Option> baseCrud, CoreDbContext db)
{
    public async Task<Result<OptionReadDto>> CreateAsync(OptionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.PollId, dto.ParticipationId, ct);
        if (!validationResult.IsSuccess)
            return Result<OptionReadDto>.Failure(validationResult.Errors);

        var entity = OptionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<OptionReadDto>.Success(OptionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, OptionUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.PollId, dto.ParticipationId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => OptionMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Option with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Option with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int pollId, int participationId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Polls.AnyAsync(p => p.Id == pollId, ct))
            errors.Add($"PollId ({pollId}) not found");

        if (!await db.Participations.AnyAsync(p => p.Id == participationId, ct))
            errors.Add($"ParticipationId ({participationId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}
