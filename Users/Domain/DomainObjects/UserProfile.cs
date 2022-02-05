using System;
using System.Collections.Generic;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserProfile
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

        //Generated Fields
        public UserBilling UserBilling { get; set; } = null;
        public List<Semester> Semesters { get; set; } = null;
    }
}