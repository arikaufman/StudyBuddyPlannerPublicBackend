using System.Collections.Generic;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Admin.Domain
{
    public interface IAdminService
    {
        SmokeTest CreateSmokeTest(SmokeTest smokeTest);
        void CreateErrorLogEntry(ErrorLog errorLog);
        void CreateAccessLogEntry(AccessLog accessLog);
        void SendEmail(string emailFileExtension, string firstName, string lastName, string email,
            string friendFirstName = "", string friendLastName = "", string friendUniversity = "");

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
