using System.Collections.Generic;

namespace plannerBackEnd.Schools.Domain
{
    public interface ISchoolService
    {
        School Get(int requestedId);
        List<School> GetList();
    }
}