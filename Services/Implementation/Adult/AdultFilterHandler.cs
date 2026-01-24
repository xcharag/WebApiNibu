using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Adult;

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

        return query;
    }
}
