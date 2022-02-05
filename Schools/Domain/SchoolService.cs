using plannerBackEnd.Schools.DataAccess;
using System.Collections.Generic;

namespace plannerBackEnd.Schools.Domain
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolDataAccessor schoolDataAccessor;

        // -----------------------------------------------------------------------------

        public SchoolService(
            ISchoolDataAccessor schoolDataAccessor
        )
        {
            this.schoolDataAccessor = schoolDataAccessor;
        }

        // -----------------------------------------------------------------------------

        public School Get(int requestedId)
        {
            return schoolDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public List<School> GetList()
        {
            return schoolDataAccessor.GetList();
        }

    }
}