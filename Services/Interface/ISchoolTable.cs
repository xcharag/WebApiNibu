using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Interface;

public interface ISchoolTable
{
    Task<IReadOnlyList<SchoolTableReadDto>> QueryAsync(SchoolTableQuery query, CancellationToken ct = default);
    Task<SchoolTableReadDto?> GetByIdAsync(int id, bool includeContacts = true, bool includeStudents = true, CancellationToken ct = default);

    Task<SchoolTableReadDto> CreateAsync(SchoolTableCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, SchoolTableUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}