using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;
using ClosedXML.Excel;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public class TournamentRosterCommands(IBaseCrud<Data.Entity.CopaUpsa.TournamentRoster> baseCrud, CoreDbContext db)
{
    public async Task<Result<TournamentRosterReadDto>> CreateAsync(TournamentRosterCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.TournamentId, dto.SchoolId, ct);
        if (!validation.IsSuccess)
            return Result<TournamentRosterReadDto>.Failure(validation.Errors);

        var entity = TournamentRosterMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);

        await db.Entry(created).Reference(tr => tr.Tournament).LoadAsync(ct);
        await db.Entry(created).Reference(tr => tr.SchoolTable).LoadAsync(ct);

        return Result<TournamentRosterReadDto>.Success(TournamentRosterMapper.ToReadDto(created));
    }

    public async Task<Result<TournamentRosterUploadResultDto>> UploadFromExcel(IFormFile file, CancellationToken ct)
    {
        var result = new TournamentRosterUploadResultDto();

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1);

        if (rows is null)
            return Result<TournamentRosterUploadResultDto>.Success(result);

        // Pre-load participations with their school for efficient lookup
        var participations = await db.Participations
            .Include(p => p.SchoolTable)
            .Where(p => p.Active)
            .ToListAsync(ct);

        var rowNumber = 1;
        foreach (var row in rows)
        {
            rowNumber++;
            var rowErrors = new List<string>();

            // Column 1: Nombre (FirstName)
            var firstName = row.Cell(1).GetString().Trim();
            // Column 2: Segundo Nombre (MiddleName)
            var middleName = row.Cell(2).GetString().Trim();
            // Column 3: Apellido Paterno (LastName)
            var lastName = row.Cell(3).GetString().Trim();
            // Column 4: Apellido Materno (MaternalName)
            var maternalName = row.Cell(4).GetString().Trim();
            // Column 5: NroDocumento (DocumentNumber)
            var documentNumber = row.Cell(5).GetString().Trim();
            // Column 6: TorneoId
            var torneoIdStr = row.Cell(6).GetString().Trim();
            // Column 7: Colegio (school name)
            var colegioName = row.Cell(7).GetString().Trim();

            if (string.IsNullOrWhiteSpace(firstName))
                rowErrors.Add($"Fila {rowNumber}: 'Nombre' es obligatorio.");

            if (string.IsNullOrWhiteSpace(lastName))
                rowErrors.Add($"Fila {rowNumber}: 'Apellido Paterno' es obligatorio.");

            if (!int.TryParse(torneoIdStr, out var torneoId))
                rowErrors.Add($"Fila {rowNumber}: TorneoId '{torneoIdStr}' no es un número válido.");

            if (string.IsNullOrWhiteSpace(colegioName))
                rowErrors.Add($"Fila {rowNumber}: 'Colegio' es obligatorio.");

            if (rowErrors.Count > 0)
            {
                result.Errors.AddRange(rowErrors);
                continue;
            }

            // Search for a participation in the tournament whose school name contains the input
            var matchingParticipation = participations.FirstOrDefault(p =>
                p.TournamentId == torneoId &&
                p.SchoolTable.Name.Contains(colegioName, StringComparison.OrdinalIgnoreCase) &&
                p.Active);

            if (matchingParticipation is null)
            {
                rowErrors.Add($"Fila {rowNumber}: No se encontró un colegio con participación en el torneo {torneoId} cuyo nombre contenga '{colegioName}'.");
                result.Errors.AddRange(rowErrors);
                continue;
            }

            var entity = new Data.Entity.CopaUpsa.TournamentRoster
            {
                FirstName = firstName,
                MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName,
                LastName = lastName,
                MaternalName = string.IsNullOrWhiteSpace(maternalName) ? null : maternalName,
                DocumentNumber = string.IsNullOrWhiteSpace(documentNumber) ? null : documentNumber,
                TournamentId = torneoId,
                Tournament = null!,
                SchoolId = matchingParticipation.SchoolId,
                SchoolTable = null!,
                Active = true
            };

            var created = await baseCrud.CreateAsync(entity, ct);

            await db.Entry(created).Reference(tr => tr.Tournament).LoadAsync(ct);
            await db.Entry(created).Reference(tr => tr.SchoolTable).LoadAsync(ct);

            result.Created.Add(TournamentRosterMapper.ToReadDto(created));
        }

        return Result<TournamentRosterUploadResultDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, TournamentRosterUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.TournamentId.HasValue && !await db.Tournaments.AnyAsync(x => x.Id == dto.TournamentId.Value, ct))
            errors.Add($"TournamentId ({dto.TournamentId.Value}) not found");
        if (dto.SchoolId.HasValue && !await db.Schools.AnyAsync(x => x.Id == dto.SchoolId.Value, ct))
            errors.Add($"SchoolId ({dto.SchoolId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => TournamentRosterMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentRoster with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentRoster with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int tournamentId, int schoolId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Tournaments.AnyAsync(x => x.Id == tournamentId, ct))
            errors.Add($"TournamentId ({tournamentId}) not found");
        if (!await db.Schools.AnyAsync(x => x.Id == schoolId, ct))
            errors.Add($"SchoolId ({schoolId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}

