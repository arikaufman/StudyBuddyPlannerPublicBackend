using System;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.sqlTools;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Admin.DataAccess.Entities;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Personals.DataAccess.Entities;

namespace plannerBackEnd.Admin.DataAccess.Dao
{
    public class AdminChartsDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public AdminChartsDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // -----------------------------------------------------------------------------

        public AdminStatisticsEntity GetListAdminStats()
        {
            string query = @"SELECT 

                                CAST((SELECT COUNT(*) AS 'value1' FROM userprofile WHERE major != 'fakeuser') AS decimal) AS `NumberOfUsers`,

                                CAST((SELECT COUNT(*) AS `value1`
                                FROM schools S WHERE (SELECT COUNT(*) FROM userprofile WHERE schoolId = S.id) > 0 ) AS decimal) AS `NumberOfUniversities`,

                                (SELECT COUNT(DISTINCT userid) AS 'value1' FROM subjects WHERE (SELECT major from userprofile UP
                                WHERE UP.id = userid) != 'fakeuser')/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser') AS `PercentUsersAddedSubjects`,

                                (SELECT COUNT(DISTINCT userid) AS 'value1' FROM tasks WHERE minutes > 0 AND (SELECT major from userprofile UP
                                WHERE UP.id = userid) != 'fakeuser')/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser') AS `PercentUsersAddedTasksAndTime`,

                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= DATE_SUB(DATE(now()), INTERVAL  2 day) AND 
                                (SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id GROUP BY T.userid) > 600)/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id GROUP BY T.userid) > 600) AS `TenHrActivityRatio`,

                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= DATE_SUB(DATE(now()), INTERVAL  2 day) AND 
                                (SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id GROUP BY T.userid) > 0)/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id GROUP BY T.userid) > 0) AS `ZeroToTenHrActivityRatio`,

                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= DATE_SUB(DATE(now()), INTERVAL  2 day) AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week))/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week)) AS `DAU`,

                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= '2020-10-25' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week))/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week)) AS `MAU`,

                                ((SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= '2020-11-25' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week))/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week)))/
                                ((SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND (SELECT lastactive FROM 
                                useractivity UA WHERE UA.userid = UP.id) >= '2020-10-25' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week))/
                                (SELECT COUNT(*) FROM userprofile UP WHERE major != 'fakeuser' AND rowcreated < DATE_SUB(DATE(now()), INTERVAL  1 week))) AS `DAUMAURatio`";

            DataRow dataRow = sqlTools.GetDataRow(query);

            AdminStatisticsEntity returnRow = new AdminStatisticsEntity();

            PropertyInfo[] properties = typeof(AdminStatisticsEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<UserAnalysisEntity> GetListUsers()
        {

            List<UserAnalysisEntity> filterResponse = new List<UserAnalysisEntity>();

            string query = @"SELECT CONCAT(UP.firstname, ' ', UP.lastname) AS 'name',
                                    IFNULL((SELECT name from schools WHERE id = UP.schoolid),'N/A')  AS 'school',
                                    IF((SELECT COUNT(*) FROM subjects S WHERE S.userid = UP.id) > 0, 'Yes', 'No') AS 'hassubjects',
                                    IF((SELECT COUNT(*) FROM tasks T WHERE T.userid = UP.id) > 0, 'Yes', 'No') AS 'hastasks',
                                    IF((SELECT SUM(T.minutes) FROM tasks T WHERE T.userid = UP.id) > 0, 'Yes', 'No') AS 'hastime',
				                    IF((SELECT COUNT(*) FROM friends F WHERE F.userid1 = UP.id OR F.userid2 = UP.id AND F.status = 2) > 0, 'Yes', 'No') AS 'hasfriends',
                                    IFNULL((SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id),0) AS 'totalloggedminutes',
                                    (SELECT COUNT(*) FROM friends F WHERE F.userid1 = UP.id OR F.userid2 = UP.id) AS 'numberoffriends'
                                             FROM userprofile UP
                                                 WHERE UP.major != 'fakeuser' ORDER BY id DESC; ";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> {});

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<UserAnalysisEntity>(dataTable);
            }
            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListNewUsers()
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            string query = @"SELECT COUNT(*) AS `value1`, rowcreated AS `date1` 
                                FROM dbplanner.userprofile WHERE major != 'fakeuser' GROUP BY DATE(rowcreated) ORDER BY rowcreated ASC; ";


            DataTable dataTable = sqlTools.GetTable(query);

            if (dataTable != null)
            {
                filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
            }

            return filterResponse;
        }
    }
}
