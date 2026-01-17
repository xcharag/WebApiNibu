using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Interface;

public interface ISport
{
    Task<IReadOnlyList<SportDto>> QueryAsync(SportQuery query, CancellationToken ct = default);
    Task<SportDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<Result<SportDto>> CreateAsync(CreateSportCommand command, CancellationToken ct = default);
    Task<Result> UpdateAsync(int id, UpdateSportCommand command, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}
