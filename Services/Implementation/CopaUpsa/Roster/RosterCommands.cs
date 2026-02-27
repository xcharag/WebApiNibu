using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public class RosterCommands(IBaseCrud<Data.Entity.CopaUpsa.Roster> baseCrud, CoreDbContext db)
{
    public async Task<Result<RosterReadDto>> CreateAsync(RosterCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.MatchId, dto.SchoolStudentId, dto.PositionId, ct);
        if (!validation.IsSuccess)
            return Result<RosterReadDto>.Failure(validation.Errors);

        var entity = RosterMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<RosterReadDto>.Success(RosterMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, RosterUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.MatchId.HasValue && !await db.Matches.AnyAsync(x => x.Id == dto.MatchId.Value, ct))
            errors.Add($"MatchId ({dto.MatchId.Value}) not found");
        if (dto.SchoolStudentId.HasValue && !await db.SchoolStudents.AnyAsync(x => x.Id == dto.SchoolStudentId.Value, ct))
            errors.Add($"SchoolStudentId ({dto.SchoolStudentId.Value}) not found");
        if (dto.PositionId.HasValue && !await db.Positions.AnyAsync(x => x.Id == dto.PositionId.Value, ct))
            errors.Add($"PositionId ({dto.PositionId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => RosterMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Roster with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Roster with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int matchId, int schoolStudentId, int positionId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Matches.AnyAsync(x => x.Id == matchId, ct))
            errors.Add($"MatchId ({matchId}) not found");
        if (!await db.SchoolStudents.AnyAsync(x => x.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        if (!await db.Positions.AnyAsync(x => x.Id == positionId, ct))
            errors.Add($"PositionId ({positionId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}

