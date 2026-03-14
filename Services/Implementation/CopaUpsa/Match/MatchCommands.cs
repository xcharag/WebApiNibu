using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;
using ClosedXML.Excel;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public class MatchCommands(IBaseCrud<Data.Entity.CopaUpsa.Match> baseCrud, CoreDbContext db)
{
    public async Task<Result<MatchReadDto>> CreateAsync(MatchCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.ParticipationAId, dto.ParticipationBId, dto.MatchStatusId, ct);
        if (!validation.IsSuccess)
            return Result<MatchReadDto>.Failure(validation.Errors);

        var entity = MatchMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);

        await db.Entry(created).Reference(m => m.ParticipationA).Query().Include(p => p.SchoolTable).LoadAsync(ct);
        await db.Entry(created).Reference(m => m.ParticipationB).Query().Include(p => p.SchoolTable).LoadAsync(ct);
        await db.Entry(created).Reference(m => m.MatchStatus).LoadAsync(ct);

        return Result<MatchReadDto>.Success(MatchMapper.ToReadDto(created));
    }

    public async Task<Result<MatchUploadResultDto>> UploadFromExcel(IFormFile file, CancellationToken ct)
    {
        var result = new MatchUploadResultDto();

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1);

        if (rows is null)
            return Result<MatchUploadResultDto>.Success(result);

        // Pre-load participations with their school names for efficient lookup
        var participations = await db.Participations
            .Include(p => p.SchoolTable)
            .Where(p => p.Active)
            .ToListAsync(ct);

        // Get the first MatchStatus as default (e.g. "Pendiente")
        var defaultStatus = await db.MatchStatuses.FirstOrDefaultAsync(ct);
        if (defaultStatus is null)
            return Result<MatchUploadResultDto>.Failure("No existe ningún estado de partido (MatchStatus) registrado.");

        var rowNumber = 1;
        foreach (var row in rows)
        {
            rowNumber++;
            var rowErrors = new List<string>();

            // Column 1: Fecha (dd-MM-yyyy)
            var fechaStr = row.Cell(1).GetString().Trim();
            // Column 2: Hora (HH:mm:ss)
            var horaStr = row.Cell(2).GetString().Trim();
            // Column 3: TorneoId
            var torneoIdStr = row.Cell(3).GetString().Trim();
            // Column 4: ColegioA (name)
            var colegioAName = row.Cell(4).GetString().Trim();
            // Column 5: ColegioB (name)
            var colegioBName = row.Cell(5).GetString().Trim();
            // Column 6: NumeroFecha
            var numeroFechaStr = row.Cell(6).GetString().Trim();

            // Parse date and time
            if (!DateTime.TryParseExact(fechaStr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var fecha))
            {
                rowErrors.Add($"Fila {rowNumber}: Fecha '{fechaStr}' no tiene el formato válido (dd-MM-yyyy).");
            }

            if (!TimeSpan.TryParseExact(horaStr, "hh\\:mm\\:ss", null, out var hora))
            {
                rowErrors.Add($"Fila {rowNumber}: Hora '{horaStr}' no tiene el formato válido (hh:mm:ss).");
            }

            if (!int.TryParse(torneoIdStr, out var torneoId))
            {
                rowErrors.Add($"Fila {rowNumber}: TorneoId '{torneoIdStr}' no es un número válido.");
            }

            if (!int.TryParse(numeroFechaStr, out var numeroFecha))
            {
                rowErrors.Add($"Fila {rowNumber}: NumeroFecha '{numeroFechaStr}' no es un número válido.");
            }

            // If basic parsing failed, skip participation lookup
            if (rowErrors.Count > 0)
            {
                result.Errors.AddRange(rowErrors);
                continue;
            }

            // Resolve participation A
            var participationA = participations.FirstOrDefault(p =>
                p.TournamentId == torneoId &&
                p.SchoolTable.Name.Equals(colegioAName, StringComparison.OrdinalIgnoreCase));

            if (participationA is null)
                rowErrors.Add($"Fila {rowNumber}: '{colegioAName}' no tiene una participación en el torneo {torneoId}.");

            // Resolve participation B
            var participationB = participations.FirstOrDefault(p =>
                p.TournamentId == torneoId &&
                p.SchoolTable.Name.Equals(colegioBName, StringComparison.OrdinalIgnoreCase));

            if (participationB is null)
                rowErrors.Add($"Fila {rowNumber}: '{colegioBName}' no tiene una participación en el torneo {torneoId}.");

            if (rowErrors.Count > 0)
            {
                result.Errors.AddRange(rowErrors);
                continue;
            }

            var startDate = DateTime.SpecifyKind(fecha.Add(hora), DateTimeKind.Utc);
            var entity = new Data.Entity.CopaUpsa.Match
            {
                Location = string.Empty,
                StartDate = startDate,
                EndDate = startDate,
                NumberMatch = numeroFecha,
                ParticipationAId = participationA!.Id,
                ParticipationA = null!,
                ParticipationBId = participationB!.Id,
                ParticipationB = null!,
                MatchStatusId = defaultStatus.Id,
                MatchStatus = null!,
                Active = true
            };

            var created = await baseCrud.CreateAsync(entity, ct);

            await db.Entry(created).Reference(m => m.ParticipationA).Query().Include(p => p.SchoolTable).LoadAsync(ct);
            await db.Entry(created).Reference(m => m.ParticipationB).Query().Include(p => p.SchoolTable).LoadAsync(ct);
            await db.Entry(created).Reference(m => m.MatchStatus).LoadAsync(ct);

            result.Created.Add(MatchMapper.ToReadDto(created));
        }

        return Result<MatchUploadResultDto>.Success(result);
    }

    public async Task<Result<MatchResultUploadResultDto>> UploadResultsFromExcel(IFormFile file, CancellationToken ct)
    {
        var result = new MatchResultUploadResultDto();

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1);

        if (rows is null)
            return Result<MatchResultUploadResultDto>.Success(result);

        var rowNumber = 1;
        foreach (var row in rows)
        {
            rowNumber++;
            var rowErrors = new List<string>();

            // Column 1: PartidoId
            var partidoIdStr = row.Cell(1).GetString().Trim();
            // Column 2: ResultadoEquipoA
            var scoreAStr = row.Cell(2).GetString().Trim();
            // Column 3: ResultadoEquipoB
            var scoreBStr = row.Cell(3).GetString().Trim();
            // Column 4: DetallePuntosA (optional)
            var detailAStr = row.Cell(4).GetString().Trim();
            // Column 5: DetallePuntosB (optional)
            var detailBStr = row.Cell(5).GetString().Trim();

            if (!int.TryParse(partidoIdStr, out var partidoId))
            {
                rowErrors.Add($"Fila {rowNumber}: PartidoId '{partidoIdStr}' no es un número válido.");
                result.Errors.AddRange(rowErrors);
                continue;
            }

            if (!decimal.TryParse(scoreAStr, out var scoreA))
            {
                rowErrors.Add($"Fila {rowNumber}: ResultadoEquipoA '{scoreAStr}' no es un número válido.");
            }

            if (!decimal.TryParse(scoreBStr, out var scoreB))
            {
                rowErrors.Add($"Fila {rowNumber}: ResultadoEquipoB '{scoreBStr}' no es un número válido.");
            }

            decimal? detailA = null;
            if (!string.IsNullOrWhiteSpace(detailAStr))
            {
                if (!decimal.TryParse(detailAStr, out var parsedDetailA))
                    rowErrors.Add($"Fila {rowNumber}: DetallePuntosA '{detailAStr}' no es un número válido.");
                else
                    detailA = parsedDetailA;
            }

            decimal? detailB = null;
            if (!string.IsNullOrWhiteSpace(detailBStr))
            {
                if (!decimal.TryParse(detailBStr, out var parsedDetailB))
                    rowErrors.Add($"Fila {rowNumber}: DetallePuntosB '{detailBStr}' no es un número válido.");
                else
                    detailB = parsedDetailB;
            }

            if (rowErrors.Count > 0)
            {
                result.Errors.AddRange(rowErrors);
                continue;
            }

            var match = await db.Matches
                .Include(m => m.ParticipationA).ThenInclude(p => p.SchoolTable)
                .Include(m => m.ParticipationB).ThenInclude(p => p.SchoolTable)
                .Include(m => m.MatchStatus)
                .FirstOrDefaultAsync(m => m.Id == partidoId && m.Active, ct);

            if (match is null)
            {
                result.Errors.Add($"Fila {rowNumber}: Partido con Id {partidoId} no fue encontrado.");
                continue;
            }

            match.ScoreA = scoreA;
            match.ScoreB = scoreB;
            if (detailA.HasValue) match.DetailPointA = detailA.Value;
            if (detailB.HasValue) match.DetailPointB = detailB.Value;

            await db.SaveChangesAsync(ct);

            result.Updated.Add(MatchMapper.ToReadDto(match));
        }

        return Result<MatchResultUploadResultDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, MatchUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.ParticipationAId.HasValue && !await db.Participations.AnyAsync(x => x.Id == dto.ParticipationAId.Value, ct))
            errors.Add($"ParticipationAId ({dto.ParticipationAId.Value}) not found");
        if (dto.ParticipationBId.HasValue && !await db.Participations.AnyAsync(x => x.Id == dto.ParticipationBId.Value, ct))
            errors.Add($"ParticipationBId ({dto.ParticipationBId.Value}) not found");
        if (dto.MatchStatusId.HasValue && !await db.MatchStatuses.AnyAsync(x => x.Id == dto.MatchStatusId.Value, ct))
            errors.Add($"MatchStatusId ({dto.MatchStatusId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => MatchMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Match with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Match with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int participationAId, int participationBId, int matchStatusId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Participations.AnyAsync(x => x.Id == participationAId, ct))
            errors.Add($"ParticipationAId ({participationAId}) not found");
        if (!await db.Participations.AnyAsync(x => x.Id == participationBId, ct))
            errors.Add($"ParticipationBId ({participationBId}) not found");
        if (!await db.MatchStatuses.AnyAsync(x => x.Id == matchStatusId, ct))
            errors.Add($"MatchStatusId ({matchStatusId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}

