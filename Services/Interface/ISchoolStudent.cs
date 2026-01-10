using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Interface;

public interface ISchoolStudent
{
    Task<IReadOnlyList<SchoolStudentReadDto>> QueryAsync(SchoolStudentQuery query, CancellationToken ct = default);
    Task<SchoolStudentReadDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<SchoolStudentReadDto> CreateAsync(CreateSchoolStudentCommand command, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateSchoolStudentCommand command, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}