using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.SchoolStudent;

public static class SchoolStudentFilterHandler
{
    public static IQueryable<Data.Entity.Person.SchoolStudent> Apply(
        IQueryable<Data.Entity.Person.SchoolStudent> query, SchoolStudentFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var pattern = $"%{filter.Name.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.FirstName, pattern) ||
                (x.MiddleName != null && EF.Functions.ILike(x.MiddleName, pattern)) ||
                EF.Functions.ILike(x.PaternalSurname, pattern) ||
                EF.Functions.ILike(x.MaternalSurname, pattern));
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(x => x.Email.Contains(filter.Email));

        if (filter.IdCountry.HasValue)
            query = query.Where(x => x.IdCountry == filter.IdCountry.Value);

        if (filter.IdDocumentType.HasValue)
            query = query.Where(x => x.IdDocumentType == filter.IdDocumentType.Value);

        if (filter.IdSchool.HasValue)
            query = query.Where(x => x.IdSchool == filter.IdSchool.Value);

        if (filter.IsPlayer.HasValue)
            query = query.Where(x => x.IsPlayer == filter.IsPlayer.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
