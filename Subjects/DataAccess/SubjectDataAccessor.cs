using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Subjects.DataAccess.Dao;
using plannerBackEnd.Subjects.DataAccess.Entities;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Subjects.DataAccess
{
    public class SubjectDataAccessor : ISubjectDataAccessor
    {
        private readonly IMapper mapper;
        private readonly SubjectDao subjectDao;


        // -----------------------------------------------------------------------------

        public SubjectDataAccessor(IMapper mapper, SubjectDao subjectDao)
        {
            this.mapper = mapper;
            this.subjectDao = subjectDao;
        }

        // -----------------------------------------------------------------------------

        public Subject Get(int requestedId)
        {
            return mapper.Map<SubjectEntity, Subject>(subjectDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<Subject> GetList(SubjectFilterRequest filter)
        {
            return mapper.Map<List<SubjectEntity>, List<Subject>>
                (subjectDao.GetList(filter));
        }

        // -----------------------------------------------------------------------------

        public Subject Create(Subject subject)
        {
            return mapper.Map<SubjectEntity, Subject>(subjectDao.Create(mapper.Map<Subject, SubjectEntity>(subject)));
        }

        // -----------------------------------------------------------------------------

        public Subject Update(Subject subject)
        {
            return mapper.Map<SubjectEntity, Subject>(subjectDao.Update(mapper.Map<Subject, SubjectEntity>(subject)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int subjectId)
        {
            return subjectDao.Delete(subjectId);
        }

    }
}