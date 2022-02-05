namespace plannerBackEnd.Admin.Domain.DomainObjects
{
    public class UserAnalysis
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