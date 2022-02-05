using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.DataAccess.Entities
{
    public class UserAnalysisEntity : IMaps<UserAnalysis>
    {
        public string Name { get; set; } = "";
        public string School { get; set; } = "";
        public string HasSubjects { get; set; } = "";
        public string HasTasks { get; set; } = "";
        public string HasTime { get; set; } = "";
        public string HasFriends { get; set; } = "";
        public double TotalLoggedMinutes { get; set; } = 0;
        public double NumberOfFriends { get; set; } = 0;
    }
}