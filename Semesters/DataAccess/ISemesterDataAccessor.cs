using System.Collections.Generic;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Semesters.DataAccess
{
    public interface ISemesterDataAccessor
    {
        Semester Get(int requestedId);
        List<Semester> GetList(SemesterFilterRequest filter);
        Semester Create(Semester semester);
        Semester Update(Semester semester);
        bool Delete(int requestedId);
    }
}