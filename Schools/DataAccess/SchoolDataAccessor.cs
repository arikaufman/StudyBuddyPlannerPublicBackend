using AutoMapper;
using plannerBackEnd.Schools.DataAccess.Dao;
using plannerBackEnd.Schools.DataAccess.Entities;
using System.Collections.Generic;

namespace plannerBackEnd.Schools.DataAccess
{
    public class SchoolDataAccessor : ISchoolDataAccessor
    {
        private readonly IMapper mapper;
        private readonly SchoolDao schoolDao;

        // -----------------------------------------------------------------------------

        public SchoolDataAccessor(IMapper mapper, SchoolDao schoolDao)
        {
            this.mapper = mapper;
            this.schoolDao = schoolDao;
        }

        // -----------------------------------------------------------------------------

        public School Get(int requestedId)
        {
            return mapper.Map<SchoolEntity, School>(schoolDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<School> GetList()
        {
            return mapper.Map<List<SchoolEntity>, List<School>>(schoolDao.GetList());
        }
    }
}