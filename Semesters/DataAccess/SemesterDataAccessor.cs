using AutoMapper;
using plannerBackEnd.Semesters.DataAccess.Dao;
using plannerBackEnd.Semesters.DataAccess.Entities;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Semesters.DataAccess
{
    public class SemesterDataAccessor : ISemesterDataAccessor
    {
        private readonly IMapper mapper;
        private readonly SemesterDao semesterDao;

        // -----------------------------------------------------------------------------

        public SemesterDataAccessor(IMapper mapper, SemesterDao semesterDao)
        {
            this.mapper = mapper;
            this.semesterDao = semesterDao;
        }

        // -----------------------------------------------------------------------------

        public Semester Get(int requestedId)
        {
            return mapper.Map<SemesterEntity, Semester>(semesterDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<Semester> GetList(SemesterFilterRequest filter)
        {
            return mapper.Map<List<SemesterEntity>, List<Semester>>
                (semesterDao.GetList(filter));
        }

        // -----------------------------------------------------------------------------

        public Semester Create(Semester semester)
        {
            return mapper.Map<SemesterEntity, Semester>(semesterDao.Create(mapper.Map<Semester, SemesterEntity>(semester)));
        }

        // -----------------------------------------------------------------------------

        public Semester Update(Semester semester)
        {
            return mapper.Map<SemesterEntity, Semester>(semesterDao.Update(mapper.Map<Semester, SemesterEntity>(semester)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int semesterId)
        {
            return semesterDao.Delete(semesterId);
        }

    }
}