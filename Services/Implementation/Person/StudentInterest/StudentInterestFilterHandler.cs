using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.StudentInterest;

public static class StudentInterestFilterHandler
{
    public static IQueryable<Data.Entity.Person.StudentInterest> Apply(
        IQueryable<Data.Entity.Person.StudentInterest> query, StudentInterestFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.InterestActivityId.HasValue)
            query = query.Where(x => x.IdInterestActivity == filter.InterestActivityId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
