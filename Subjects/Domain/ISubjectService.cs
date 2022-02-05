using plannerBackEnd.Subjects.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Subjects.Domain
{
    public interface ISubjectService
    {
        Subject Get(int requestedId);
        List<Subject> GetList(SubjectFilterRequest filter);
        Subject Create(Subject subject);
        Subject Update(Subject subject);
        bool Delete(int requestedId);

    }
}