using plannerBackEnd.Subjects.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Subjects.DataAccess
{
    public interface ISubjectDataAccessor
    {
        Subject Get(int requestedId);
        List<Subject> GetList(SubjectFilterRequest filter);
        Subject Create(Subject subject);
        Subject Update(Subject subject);
        bool Delete(int requestedId);
    }
}