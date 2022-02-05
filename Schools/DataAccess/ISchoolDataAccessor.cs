using System.Collections.Generic;

namespace plannerBackEnd.Schools.DataAccess
{
    public interface ISchoolDataAccessor
    {
        School Get(int requestedId);
        List<School> GetList();
    }
}