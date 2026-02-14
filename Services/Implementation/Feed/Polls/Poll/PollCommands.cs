using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public class PollCommands(IBaseCrud<Data.Entity.Feed.Polls.Poll> baseCrud, OracleDbContext db)
{
    public async Task<Result<PollReadDto>> CreateAsync(PollCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.TournamentId, ct);
        if (!validationResult.IsSuccess)
            return Result<PollReadDto>.Failure(validationResult.Errors);

        var entity = PollMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<PollReadDto>.Success(PollMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, PollUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.TournamentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => PollMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Poll with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Poll with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int tournamentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Tournaments.AnyAsync(t => t.Id == tournamentId, ct))
            errors.Add($"TournamentId ({tournamentId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}
