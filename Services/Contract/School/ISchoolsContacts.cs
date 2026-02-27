using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.School;

public interface ISchoolsContacts
{
    // Queries
    Task<Result<PagedResult<ContactReadDto>>> GetAllAsync(ContactFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<ContactReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<ContactReadDto>> CreateAsync(ContactCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, ContactUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}