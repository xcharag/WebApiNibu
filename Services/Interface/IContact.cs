using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Interface;

public interface IContact
{
    Task<IReadOnlyList<ContactReadDto>> QueryAsync(ContactQuery query, CancellationToken ct = default);
    Task<ContactReadDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<ContactReadDto> CreateAsync(CreateContactCommand command, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateContactCommand command, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}