using plannerBackEnd.Admin.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Admin.DataAccess
{
    public interface IAdminDataAccessor
    {
       SmokeTest CreateSmokeTest(SmokeTest smokeTest);
       void CreateErrorLogEntry(ErrorLog errorLog);
       void CreateAccessLogEntry(AccessLog accessLog);

        // SUPPORT LOG
        SupportLog Get(int requestedId);
       List<SupportLog> GetList();
       SupportLog Create(SupportLog supportLog);
       SupportLog Update(SupportLog supportLog);
       bool Delete(int requestedId);

        // CHARTS
        AdminStatistics GetListAdminStats();
       List<UserAnalysis> GetListUsers();
       BaseFilterResponse GetListNewUsers();

    }
}
