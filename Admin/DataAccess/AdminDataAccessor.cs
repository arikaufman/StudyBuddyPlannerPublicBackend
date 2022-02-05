using AutoMapper;
using plannerBackEnd.Admin.DataAccess.Dao;
using plannerBackEnd.Admin.DataAccess.Entities;
using plannerBackEnd.Admin.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Admin.DataAccess
{
    public class AdminDataAccessor : IAdminDataAccessor
    {
        private readonly IMapper mapper;
        private readonly AdminDao adminDao;
        private readonly AdminChartsDao adminChartsDao;
        private readonly SupportLogDao supportLogDao;

        // -----------------------------------------------------------------------------

        public AdminDataAccessor(IMapper mapper, AdminDao adminDao, AdminChartsDao adminChartsDao, SupportLogDao supportLogDao)
        {
            this.mapper = mapper;
            this.adminDao = adminDao;
            this.adminChartsDao = adminChartsDao;
            this.supportLogDao = supportLogDao;
        }

        // -----------------------------------------------------------------------------

        public SmokeTest CreateSmokeTest(SmokeTest smokeTest)
        {
            return mapper.Map<SmokeTestEntity, SmokeTest>(adminDao.CreateSmokeTest(mapper.Map<SmokeTest, SmokeTestEntity>(smokeTest)));
        }

        // -----------------------------------------------------------------------------

        public void CreateErrorLogEntry(ErrorLog errorLog)
        {
            adminDao.CreateErrorLogEntry(mapper.Map<ErrorLog, ErrorLogEntity>(errorLog));
        }

        // -----------------------------------------------------------------------------

        public void CreateAccessLogEntry(AccessLog accessLog)
        {
            adminDao.CreateAccessLogEntry(mapper.Map<AccessLog, AccessLogEntity>(accessLog));
        }

        //SUPPORT LOG
        // -----------------------------------------------------------------------------

        public SupportLog Get(int requestedId)
        {
            return mapper.Map<SupportLogEntity, SupportLog>(supportLogDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<SupportLog> GetList()
        {
            return mapper.Map<List<SupportLogEntity>, List<SupportLog>>
                (supportLogDao.GetList());
        }

        // -----------------------------------------------------------------------------

        public SupportLog Create(SupportLog supportLog)
        {
            return mapper.Map<SupportLogEntity, SupportLog>(supportLogDao.Create(mapper.Map<SupportLog, SupportLogEntity>(supportLog)));
        }

        // -----------------------------------------------------------------------------

        public SupportLog Update(SupportLog supportLog)
        {
            return mapper.Map<SupportLogEntity, SupportLog>(supportLogDao.Update(mapper.Map<SupportLog, SupportLogEntity>(supportLog)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int semesterId)
        {
            return supportLogDao.Delete(semesterId);
        }

        // CHARTS
        // -----------------------------------------------------------------------------

        public AdminStatistics GetListAdminStats()
        {
            return mapper.Map < AdminStatisticsEntity, AdminStatistics >( adminChartsDao.GetListAdminStats());
        }

        // -----------------------------------------------------------------------------

        public List<UserAnalysis> GetListUsers()
        {
            return mapper.Map<List<UserAnalysisEntity>, List<UserAnalysis>>(adminChartsDao.GetListUsers());
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListNewUsers()
        {
            return adminChartsDao.GetListNewUsers();
        }
    }
}
