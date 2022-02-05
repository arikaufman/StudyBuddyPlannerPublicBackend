using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.DataAccess.Entities
{
    public class AdminStatisticsEntity : IMaps<AdminStatistics>
    {
        public decimal NumberOfUsers { get; set; } = 0;
        public decimal NumberOfUniversities { get; set; } = 0;
        public decimal PercentUsersAddedSubjects { get; set; } = 0;
        public decimal PercentUsersAddedTasksAndTime { get; set; } = 0;
        public decimal TenHrActivityRatio { get; set; } = 0;
        public decimal ZeroToTenHrActivityRatio { get; set; } = 0;
        public decimal DAU { get; set; } = 0;
        public decimal MAU { get; set; } = 0;
        public decimal DAUMAURatio { get; set; } = 0;
    }
}