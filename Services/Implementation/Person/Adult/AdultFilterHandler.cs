using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.Adult;

public static class AdultFilterHandler
{
    public static IQueryable<Data.Entity.Person.Adult> Apply(
        IQueryable<Data.Entity.Person.Adult> query, 
        AdultFilter filter)
    {
        if (filter.AdultTypeId.HasValue)
            query = query.Where(x => x.IdAdultType == filter.AdultTypeId.Value);

        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (!string.IsNullOrWhiteSpace(filter.WorkEmail))
            query = query.Where(x => x.WorkEmail.Contains(filter.WorkEmail));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        // Added: filter by adult name (first, paternal and maternal surname)
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim();
            query = query.Where(x => x.SchoolStudent.FirstName.Contains(name) ||
                                     x.SchoolStudent.PaternalSurname.Contains(name) ||
                                     x.SchoolStudent.MaternalSurname.Contains(name));
        }

        return query;
    }
}
