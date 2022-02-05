using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess.Entities
{
    public class UserProfileEntity : IMaps<UserProfile>
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int SchoolId { get; set; } = 0;
        public int FacultyId { get; set; } = 0;
        public string Major { get; set; } = "";
        public string Minor { get; set; } = "";
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordQuestion { get; set; } = "";
        public string PasswordAnswer { get; set; } = "";
        public int Year { get; set; } = 0;
        public int UsePercentage { get; set; } = 0;
        public int DarkMode { get; set; } = 0;
        public int DashboardTour { get; set; } = 0;
        public int TaskTour { get; set; } = 0;
        public int SubjectTour { get; set; } = 0;
        public int SettingTour { get; set; } = 0;
        public int CompareTour { get; set; } = 0;
        public int FeedPrivacy { get; set; } = 0;
        public int ProgressBar { get; set; } = 0;
        public string ReferredByCode { get; set; } = "";
        public DateTime TokenExpiry { get; set; } = DateTime.Now;
        public DateTime RowCreated { get; set; } = DateTime.Now;
    }
}