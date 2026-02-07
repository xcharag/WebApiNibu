using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.PreferencesStudent;

public static class PreferencesStudentFilterHandler
{
    public static IQueryable<Data.Entity.Person.PreferencesStudent> Apply(
        IQueryable<Data.Entity.Person.PreferencesStudent> query, 
        PreferencesStudentFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.HaveVocationalTest.HasValue)
            query = query.Where(x => x.HaveVocationalTest == filter.HaveVocationalTest.Value);

        if (filter.StudyAbroad.HasValue)
            query = query.Where(x => x.StudyAbroad == filter.StudyAbroad.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}
