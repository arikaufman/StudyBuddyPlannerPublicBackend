using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Users.DataAccess.Entities;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess.Dao
{
    public class UserProfileDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public UserProfileDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public UserProfileEntity Get(int requestedId)
        {
            string query = "SELECT * FROM userprofile WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            UserProfileEntity returnRow = new UserProfileEntity();

            PropertyInfo[] properties = typeof(UserProfileEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------

        public UserProfileEntity Get(string email)
        {
            string query = "SELECT * FROM userprofile WHERE email = '" + email + "'";

            DataRow dataRow = sqlTools.GetDataRow(query);


            UserProfileEntity returnRow = new UserProfileEntity();

            PropertyInfo[] properties = typeof(UserProfileEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------

        public List<UserProfileEntity> GetList(UserProfileFilterRequest filterRequest)
        {
            List<UserProfileEntity> filterResponse = new List<UserProfileEntity>();

            string query = @"SELECT UP.id, UP.firstname, UP.lastname, UP.email, UP.schoolid, UP.facultyid FROM userprofile UP WHERE UP.major != 'fakeuser'";

            if (filterRequest.Search.Length > 0)
            {
                query += @" AND INSTR(CONCAT(firstname, ' ', lastname), @search )
            AND SUBSTRING(CONCAT(firstname, ' ', lastname),1,LENGTH(@search)) = @search" ;
            }

            if (filterRequest.Active)
            {
                query += @" AND (SELECT SUM(T.minutes) FROM tasks T WHERE T.userid = UP.id GROUP BY T.userid) > 600";
            }

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@search", filterRequest.Search } });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<UserProfileEntity>(dataTable);
            }
            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public UserProfileEntity Create(UserProfileEntity userProfileEntity)
        {
            string query = @"INSERT INTO userprofile (
                            firstname,
                            lastname,
                            schoolid,
                            facultyid,
                            major,
                            minor,
                            email,
                            username,
                            password,
                            passwordquestion,
                            passwordanswer,
                            year,
                            usePercentage,
                            darkmode,
                            dashboardtour,
                            tasktour,
                            subjecttour,
                            settingtour,
                            comparetour,
                            feedprivacy,
                            progressbar,
                            referredbycode,
                            tokenexpiry) 

                            VALUES(
                            @firstname,
                            @lastname,
                            @schoolid,
                            @facultyid,
                            @major,
                            @minor,
                            @email,
                            @username,
                            @password,
                            @passwordquestion,
                            @passwordanswer,
                            @year,
                            @usePercentage,
                            @darkmode,
                            @dashboardtour,
                            @tasktour,
                            @subjecttour,
                            @settingtour,
                            @comparetour,
                            @feedprivacy,
                            @progressbar,
                            @referredbycode,
                            @tokenexpiry);  ";

            query += "SELECT * FROM userprofile WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@firstname", userProfileEntity.FirstName},
                {"@lastname", userProfileEntity.LastName},
                {"@schoolid", userProfileEntity.SchoolId},
                {"@facultyid", userProfileEntity.FacultyId},
                {"@major", userProfileEntity.Major},
                {"@minor", userProfileEntity.Minor},
                {"@email", userProfileEntity.Email},
                {"@username", userProfileEntity.UserName},
                {"@password", userProfileEntity.Password},
                {"@passwordquestion", userProfileEntity.PasswordQuestion},
                {"@passwordanswer", userProfileEntity.PasswordAnswer},
                {"@year", userProfileEntity.Year},
                {"@usePercentage", userProfileEntity.UsePercentage},
                {"@darkmode", userProfileEntity.DarkMode},
                {"@dashboardtour", userProfileEntity.DashboardTour},
                {"@tasktour", userProfileEntity.TaskTour},
                {"@subjecttour", userProfileEntity.SubjectTour},
                {"@settingtour", userProfileEntity.SettingTour},
                {"@comparetour", userProfileEntity.CompareTour},
                {"@feedprivacy", userProfileEntity.FeedPrivacy},
                {"@progressbar", userProfileEntity.ProgressBar},
                {"@referredbycode", userProfileEntity.ReferredByCode},
                {"@tokenexpiry", userProfileEntity.TokenExpiry},
            });

            UserProfileEntity returnRow = new UserProfileEntity();
            PropertyInfo[] properties = typeof(UserProfileEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }
            
            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public UserProfileEntity Update(UserProfileEntity userProfileEntity)
        {
            UserProfileEntity oldUserProfileEntity = Get(userProfileEntity.Id);

            string query = @"UPDATE userprofile SET 
                            firstname = @firstname,
                            lastname = @lastname,
                            schoolid = @schoolid,
                            facultyid = @facultyid,
                            major = @major,
                            minor = @minor,
                            email = @email,
                            username = @username,
                            password = @password,
                            passwordquestion = @passwordquestion,
                            passwordanswer = @passwordanswer,
                            year = @year,
                            usePercentage = @usePercentage,
                            darkMode = @darkMode,
                            dashboardtour = @dashboardtour,
                            tasktour = @tasktour,
                            subjecttour = @subjecttour,
                            settingtour = @settingtour,
                            comparetour = @comparetour,
                            feedprivacy = @feedprivacy,
                            progressbar = @progressbar,
                            referredbycode = @referredbycode,
                            tokenexpiry = @tokenexpiry

                            WHERE id = @id; 

                SELECT * FROM userprofile WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@firstname", userProfileEntity.FirstName},
                {"@lastname", userProfileEntity.LastName},
                {"@schoolid", userProfileEntity.SchoolId},
                {"@facultyid", userProfileEntity.FacultyId},
                {"@major", userProfileEntity.Major},
                {"@minor", userProfileEntity.Minor},
                {"@email", userProfileEntity.Email},
                {"@username", userProfileEntity.UserName},
                {"@password", userProfileEntity.Password},
                {"@passwordquestion", userProfileEntity.PasswordQuestion},
                {"@passwordanswer", userProfileEntity.PasswordAnswer},
                {"@year", userProfileEntity.Year},
                {"@id", userProfileEntity.Id},
                {"@usePercentage", userProfileEntity.UsePercentage},
                {"@darkmode", userProfileEntity.DarkMode},
                {"@dashboardtour", userProfileEntity.DashboardTour},
                {"@tasktour", userProfileEntity.TaskTour},
                {"@subjecttour", userProfileEntity.SubjectTour},
                {"@settingtour", userProfileEntity.SettingTour},
                {"@comparetour", userProfileEntity.CompareTour},
                {"@feedprivacy", userProfileEntity.FeedPrivacy},
                {"@progressbar", userProfileEntity.ProgressBar},
                {"@referredbycode", userProfileEntity.ReferredByCode},
                {"@tokenexpiry", userProfileEntity.TokenExpiry},
            });

            UserProfileEntity returnRow = new UserProfileEntity();
            PropertyInfo[] properties = typeof(UserProfileEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------
        public bool Delete(int requestedId)
        {
            string query = "DELETE FROM userprofile WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object>{{ "@id", requestedId }});
            


            return (result > 0 ? true : false);
        }
    }
}