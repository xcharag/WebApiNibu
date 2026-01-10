using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Data.Entity.School;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Implementation;

public class SchoolTableImplementation(IBaseCrud<SchoolTable> crud, OracleDbContext db) : ISchoolTable
{
    public async Task<IReadOnlyList<SchoolTableReadDto>> QueryAsync(SchoolTableQuery query, CancellationToken ct = default)
    {
        var q = db.Schools.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name)) q = q.Where(s => s.Name.Contains(query.Name));
        if (!string.IsNullOrWhiteSpace(query.Tier)) q = q.Where(s => s.Tier == query.Tier);
        if (!string.IsNullOrWhiteSpace(query.Ciudad)) q = q.Where(s => s.Ciudad.Contains(query.Ciudad));
        if (query.IdDepartamento.HasValue) q = q.Where(s => s.IdDepartamento == query.IdDepartamento.Value);
        if (query.Active.HasValue) q = q.Where(s => s.Active == query.Active.Value);

        if (query.IncludeContacts) q = q.Include(s => s.Contacts);
        if (query.IncludeStudents) q = q.Include(s => s.SchoolStudents);

        var items = await q.ToListAsync(ct);
        return items.Select(MapToReadDto).ToList();
    }

    public async Task<SchoolTableReadDto?> GetByIdAsync(int id, bool includeContacts = true, bool includeStudents = true, CancellationToken ct = default)
    {
        var q = db.Schools.AsNoTracking().AsQueryable();
        if (includeContacts) q = q.Include(s => s.Contacts);
        if (includeStudents) q = q.Include(s => s.SchoolStudents);

        var item = await q.FirstOrDefaultAsync(s => s.Id == id, ct);
        return item is null ? null : MapToReadDto(item);
    }

    public async Task<SchoolTableReadDto> CreateAsync(SchoolTableCreateDto dto, CancellationToken ct = default)
    {
        var entity = MapFromCreateDto(dto);

        await using var tx = await db.Database.BeginTransactionAsync(ct);
        var created = await crud.CreateAsync(entity, ct);

        if (dto.Contacts.Count > 0)
        {
            foreach (var c in dto.Contacts)
            {
                db.Contacts.Add(new Contact
                {
                    PersonName = c.PersonName,
                    PersonRole = c.PersonRole,
                    PersonPhoneNumber = c.PersonPhoneNumber,
                    PersonEmail = c.PersonEmail,
                    SchoolTable = created,
                    Active = true
                });
            }

            await db.SaveChangesAsync(ct);
        }

        await tx.CommitAsync(ct);

        // Return fresh read model with relationships
        var read = await db.Schools.AsNoTracking()
            .Include(s => s.Contacts)
            .Include(s => s.SchoolStudents)
            .FirstAsync(s => s.Id == created.Id, ct);

        return MapToReadDto(read);
    }

    public Task<bool> UpdateAsync(int id, SchoolTableUpdateDto dto, CancellationToken ct = default)
        => crud.UpdateAsync(id, entity => ApplyUpdateDto(entity, dto), ct);

    public Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
        => crud.DeleteAsync(id, softDelete, ct);

    private static SchoolTableReadDto MapToReadDto(SchoolTable s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Tier = s.Tier,
        Address = s.Address,
        SportLogo = s.SportLogo,
        NormalLogo = s.NormalLogo,
        Rue = s.Rue,
        Delegada = s.Delegada,
        Tipo = s.Tipo,
        Ciudad = s.Ciudad,
        IdDepartamento = s.IdDepartamento,
        Ka = s.Ka,
        IdDelegada = s.IdDelegada,
        IdColegio = s.IdColegio,
        KaRectorada = s.KaRectorada,
        Segemento = s.Segemento,
        Contacts = s.Contacts?.Select(MapToReadDto).ToList() ?? new List<ContactReadDto>(),
        SchoolStudents = s.SchoolStudents?.Select(MapToReadDto).ToList() ?? new List<SchoolStudentInSchoolReadDto>()
    };

    private static ContactReadDto MapToReadDto(Contact c) => new()
    {
        Id = c.Id,
        PersonName = c.PersonName,
        PersonRole = c.PersonRole,
        PersonPhoneNumber = c.PersonPhoneNumber,
        PersonEmail = c.PersonEmail
    };

    private static SchoolStudentInSchoolReadDto MapToReadDto(SchoolStudent st) => new()
    {
        Id = st.Id,
        FirstName = st.FirstName,
        MiddleName = st.MiddleName,
        PaternalSurname = st.PaternalSurname,
        MaternalSurname = st.MaternalSurname,
        DocumentNumber = st.DocumentNumber,
        BirthDate = st.BirthDate,
        PhoneNumber = st.PhoneNumber,
        Email = st.Email,
        IdCountry = st.IdCountry,
        IdDocumentType = st.IdDocumentType,
        SchoolGrade = st.SchoolGrade,
        IsPlayer = st.IsPlayer,
        HasUpsaParents = st.HasUpsaParents
    };

    private static SchoolTable MapFromCreateDto(SchoolTableCreateDto dto) => new()
    {
        Name = dto.Name,
        Tier = dto.Tier,
        Address = dto.Address,
        SportLogo = dto.SportLogo,
        NormalLogo = dto.NormalLogo,
        Rue = dto.Rue,
        Delegada = dto.Delegada,
        Tipo = dto.Tipo,
        Ciudad = dto.Ciudad,
        IdDepartamento = dto.IdDepartamento,
        Ka = dto.Ka,
        IdDelegada = dto.IdDelegada,
        IdColegio = dto.IdColegio,
        KaRectorada = dto.KaRectorada,
        Segemento = dto.Segemento,
        Active = true
    };

    private static void ApplyUpdateDto(SchoolTable target, SchoolTableUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Tier = dto.Tier;
        target.Address = dto.Address;
        target.SportLogo = dto.SportLogo;
        target.NormalLogo = dto.NormalLogo;
        target.Rue = dto.Rue;
        target.Delegada = dto.Delegada;
        target.Tipo = dto.Tipo;
        target.Ciudad = dto.Ciudad;
        target.IdDepartamento = dto.IdDepartamento;
        target.Ka = dto.Ka;
        target.IdDelegada = dto.IdDelegada;
        target.IdColegio = dto.IdColegio;
        target.KaRectorada = dto.KaRectorada;
        target.Segemento = dto.Segemento;
    }
}