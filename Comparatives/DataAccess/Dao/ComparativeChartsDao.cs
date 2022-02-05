using System;
using Common.Enums;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Faculties;
using plannerBackEnd.Schools;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Comparatives.DataAccess.Entities;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess.Dao
{
    public class ComparativeChartsDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public ComparativeChartsDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListPopulationBreakdown(BaseFilterRequest filter)
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            if (filter.BreakdownType == BreakdownType.Faculty)
            {
                filterResponse.Title = "Faculty Breakdown"; 

                string query = @"SELECT F.name AS `name1`,

                                    IFNULL((SELECT SUM(minutes) from tasks T WHERE T.userid IN 
                                    (SELECT UP.id from userprofile UP WHERE UP.facultyid = F.id AND UP.major <> 'fakeuser'
                                    AND (SELECT lastactive FROM useractivity UA WHERE UA.userid = UP.id) > DATE_SUB(curdate(), INTERVAL 1 WEEK)))/
                                    (SELECT COUNT(*) FROM userprofile UP WHERE UP.facultyid = F.id AND UP.major <> 'fakeuser'
                                    AND (SELECT lastactive FROM useractivity UA WHERE UA.userid = UP.id) > DATE_SUB(curdate(), INTERVAL 1 WEEK)),0) AS `value1`,

                                    IFNULL((SELECT SUM(minutes) FROM tasks T where T.userid = @UserId),0) AS `value2`

                                    FROM faculties F ";


                DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

                if (dataTable != null)
                {
                    filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                }
            }
            else if (filter.BreakdownType == BreakdownType.Year)
            {
                filterResponse.Title = "Year Breakdown";

                string query = @"select CONCAT('Year', ' ', year) AS `name1`,
                                    IFNULL((SELECT SUM(minutes) from tasks T WHERE T.userid IN 
                                    (SELECT UP.id from userprofile UP WHERE t.year = UP.year AND UP.major <> 'fakeuser'
                                    AND (SELECT lastactive FROM useractivity UA WHERE UA.userid = UP.id) > DATE_SUB(curdate(), INTERVAL 1 WEEK)))/
                                    (SELECT COUNT(*) FROM userprofile UP WHERE t.year = UP.year AND UP.major <> 'fakeuser'
                                    AND (SELECT lastactive FROM useractivity UA WHERE UA.userid = UP.id) > DATE_SUB(curdate(), INTERVAL 1 WEEK)),0) AS `value1`,

                                    IFNULL((SELECT COUNT(*) FROM userprofile UP WHERE t.year = UP.year AND UP.major <> 'fakeuser'
                                    AND (SELECT lastactive FROM useractivity UA WHERE UA.userid = UP.id) > DATE_SUB(curdate(), INTERVAL 1 WEEK)),0) AS `value2`,

                                    IFNULL((SELECT SUM(minutes) FROM tasks T where T.userid = @UserId),0) AS `value3`

                                    from ( select 0 AS year union all select 1 union all select 2 union all select 3 union all select 4
                                    union all select 5) t";

                DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

                if (dataTable != null)
                {
                    filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                }
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListHoursPerMonthComparative(BaseFilterRequest filter, DateTime startDate, Subject subject)
        {
            List<DateTime> mondaysFromStartToNow = new List<DateTime>();
            DateTime temporaryDate = startDate;
            while (DateTime.Now > temporaryDate)
            {
                mondaysFromStartToNow.Add(temporaryDate);
                temporaryDate = temporaryDate.AddDays(7);
            }

            List<BaseFilterResponse> filterResponse = new List<BaseFilterResponse>();

            //FILL ONE BASEFILTERRESPONSE WITH PERSONAL LINE GRAPH
            string personalQuery = @"SELECT STR_TO_DATE(CONCAT(YEARWEEK(datecompleted), ' Monday'), '%X%V %W') AS `date1`,
                                        (SELECT color FROM subjects WHERE id = @subjectId) AS `name1`,
                                        (SELECT name FROM subjects WHERE id = @subjectId) AS `name2`,
                                        IFNULL(CAST((SELECT ROUND(SUM(minutes),0)) AS double),0) AS `value1` FROM tasksessions 
                                            WHERE taskId IN (SELECT id FROM tasks WHERE subjectid = @subjectId) 
                                            GROUP BY datecompleted
                                                ORDER BY YEARWEEK(dateCompleted) ASC";

            DataTable dataTable = sqlTools.GetTable(personalQuery, new Dictionary<string, object>
            {
                {"@subjectId", filter.SubjectId }
            });

            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse = new BaseFilterResponse();
                temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);

                BaseFilterResponse personalResponse = new BaseFilterResponse(){Color = subject.Color, Title = subject.Name};

                foreach (DateTime mondayDay in mondaysFromStartToNow)
                {
                    personalResponse.ResponseItems.Add(new BaseFilterResponseItem() { Name1 = mondayDay.ToString() });
                    foreach (BaseFilterResponseItem item in temporaryResponse.ResponseItems)
                    {
                        if (item.Date1 == mondayDay)
                        {
                            personalResponse.ResponseItems.Find(x => x.Name1.Contains(mondayDay.ToString())).Value1 += item.Value1;
                        }
                    }
                }

                filterResponse.Add(personalResponse);
            }

            //FILL ONE BASEFILTERRESPONSE WITH COMPARATIVE LINE GRAPH
            string comparativeQuery = @"SELECT STR_TO_DATE(CONCAT(YEARWEEK(datecompleted), ' Monday'), '%X%V %W') AS `date1`,
                (SELECT color FROM subjects WHERE id = @subjectId) AS `name1`,
                    (SELECT CONCAT(name, ' ', classcode) FROM subjects WHERE id = @subjectId) AS `name2`,
                    IFNULL(CAST((SELECT ROUND(SUM(minutes), 0)) AS double) / (SELECT COUNT(*) FROM subjects
                         WHERE name = @name AND classcode = @classcode AND userid IN(SELECT id FROM userprofile WHERE schoolid = @schoolId)) ,0) AS `value1` FROM tasksessions
                    WHERE taskId IN(SELECT id FROM tasks WHERE subjectid IN
                        (SELECT id FROM subjects WHERE name = @name AND classcode = @classcode))
                            GROUP BY STR_TO_DATE(CONCAT(YEARWEEK(datecompleted), ' Monday'), '%X%V %W')
                            ORDER BY YEARWEEK(dateCompleted) ASC";

            dataTable = sqlTools.GetTable(comparativeQuery, new Dictionary<string, object>
            {
                {"@subjectId", filter.SubjectId },
                {"@schoolId", filter.SchoolId },
                {"@name", subject.Name},
                {"@classcode", subject.ClassCode }
            });

            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse = new BaseFilterResponse();
                temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);

                BaseFilterResponse comparativeResponse = new BaseFilterResponse(){ Color = "656565", Title = "Average" };

                foreach (DateTime mondayDay in mondaysFromStartToNow)
                {
                    comparativeResponse.ResponseItems.Add(new BaseFilterResponseItem() { Name1 = mondayDay.ToString() });
                    foreach (BaseFilterResponseItem item in temporaryResponse.ResponseItems)
                    {
                        if (item.Date1 == mondayDay)
                        {
                            comparativeResponse.ResponseItems.Find(x => x.Name1.Contains(mondayDay.ToString())).Value1 += item.Value1;
                        }
                    }
                }

                filterResponse.Add(comparativeResponse);
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public List<BestDayEntity> GetListBestDay(BaseFilterRequest filter)
        {
            List<BestDayEntity> filterResponse = new List<BestDayEntity>();

            string query = @"SELECT 
                                UP.id, UP.firstname, UP.lastname,
                                IFNULL((SELECT MAX(`value1`) AS `value1` 
                                    FROM (SELECT FLOOR(SUM(minutes)) AS `value1`  FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = UP.id) 
                                     GROUP BY datecompleted ORDER BY value1 DESC) as t),0) AS `minutes`,
                                        (SELECT datecompleted FROM tasksessions 
                                        WHERE taskId IN (SELECT id FROM tasks WHERE userid = UP.id) 
                                        GROUP BY datecompleted ORDER BY SUM(minutes) DESC LIMIT 1) AS `bestdaydate`
                                            FROM userprofile UP
                                            WHERE
                                                (UP.id IN(SELECT userid1 FROM friends WHERE userid2 = @UserId AND status = 2) OR
                                                UP.id IN(SELECT userid2 FROM friends WHERE userid1 = @UserId AND status = 2)) OR UP.id = @UserId
                                                    ORDER BY
                                                        IFNULL((SELECT MAX(`value1`) AS `value1` 
                                                        FROM (SELECT CAST(ROUND(SUM(minutes),0) AS double) AS `value1`  FROM tasksessions 
                                                        WHERE taskId IN (SELECT id FROM tasks WHERE userid = UP.id) 
                                                             GROUP BY datecompleted ORDER BY value1 DESC) as t),0) DESC LIMIT 5; ";

            string query2 = @"SELECT (SELECT id FROM userprofile UP WHERE UP.id = (SELECT T.userid FROM tasks T WHERE T.id = TS.taskid)) AS `id`,
                                    (SELECT firstname FROM userprofile UP WHERE UP.id = (SELECT T.userid FROM tasks T WHERE T.id = TS.taskid)) AS `firstname`,
                                (SELECT lastname FROM userprofile UP WHERE UP.id = (SELECT T.userid FROM tasks T WHERE T.id = TS.taskid)) AS `lastname`,
                                SUM(minutes) AS `minutes`,
                                datecompleted AS `bestdaydate`
                                    FROM tasksessions TS WHERE (SELECT T.userid FROM tasks T WHERE T.id = TS.taskid) = @UserId GROUP BY datecompleted ORDER BY minutes DESC LIMIT 5;";

            DataTable dataTable = new DataTable();

            if (filter.Personal)
            {
                dataTable = sqlTools.GetTable(query2, new Dictionary<string, object> { { "@UserId", filter.UserId } });
            }
            else
            {
                dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });
            }

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<BestDayEntity>(dataTable);
            }

            return filterResponse;

        }

        // -----------------------------------------------------------------------------

        public List<BestAssignmentEntity> GetListBestAssignment(BaseFilterRequest filter)
        {
            List<BestAssignmentEntity> filterResponse = new List<BestAssignmentEntity>();

            string query = @"SELECT 
                                UP.firstname, UP.lastname,
                                (SELECT CONCAT(S.name, ' ', S.classcode) FROM subjects S WHERE S.userid = UP.id AND 
                                (SELECT t.subjectid FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) = S.id) AS `subjecttitle`, 
                                (SELECT S.color FROM subjects S WHERE S.userid = UP.id AND
                                (SELECT t.subjectid FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) = S.id) AS `color`,
                                (SELECT t.title FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) AS `tasktitle`,
                                (SELECT t.tasktype FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) AS `tasktype`,
                                (SELECT t.minutes FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) AS `minutes`
                                    FROM userprofile UP
                                    WHERE
                                        (UP.id IN(SELECT userid1 FROM friends WHERE userid2 = @UserId AND status = 2) OR
                                         UP.id IN(SELECT userid2 FROM friends WHERE userid1 = @UserId AND status = 2)) OR UP.id = @UserId
                                            ORDER BY
                                            (SELECT t.minutes FROM tasks t WHERE t.userid = UP.id ORDER BY minutes DESC LIMIT 1) DESC LIMIT 5; ";


            string query2 = @"SELECT (SELECT firstname FROM userprofile UP WHERE UP.id = T.userid) AS `firstname`,
                                (SELECT lastname FROM userprofile UP WHERE UP.id = T.userid) AS `lastname`,
                                (SELECT CONCAT(S.name, ' ', S.classcode) FROM subjects S WHERE T.subjectid = S.id) AS `subjecttitle`, 
                                (SELECT S.color FROM subjects S WHERE T.subjectid = S.id) AS `color`,
                                T.title AS `tasktitle`,
                                T.tasktype,
                                T.minutes
                                FROM tasks T WHERE userid = @UserId ORDER BY minutes DESC LIMIT 5;";

            DataTable dataTable = new DataTable();

            if (filter.Personal)
            {
                dataTable = sqlTools.GetTable(query2, new Dictionary<string, object> { { "@UserId", filter.UserId } });
            }
            else
            {
                dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });
            }

            if (dataTable != null)
            {

                filterResponse = sqlTools.ConvertDataTable<BestAssignmentEntity>(dataTable);
            }

            return filterResponse;

        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListMarksHoursScatter(BaseFilterRequest filter)
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            string query = @"SELECT IF((SELECT usepercentage FROM userprofile UP WHERE UP.id = @UserId) = 0, S.startgpa, S.startpercentage) AS `value1`, 
                               CAST(ROUND(IFNULL((SELECT SUM(minutes) FROM tasks T WHERE T.userid = S.userid),0),0) AS double) AS `value2` FROM semesters S WHERE 
                               IFNULL((SELECT SUM(minutes) FROM tasks T WHERE T.userid = S.userid),0) != 0
                               AND IF((SELECT usepercentage FROM userprofile UP WHERE UP.id = @UserId) = 0, S.startgpa, S.startpercentage) != 0; ";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

            if (dataTable != null)
            {

                filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
            }

            return filterResponse;

        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListSchoolFacultyScatter(BaseFilterRequest filter)
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            //Get List of School Ids
            string schoolQuery = @" SELECT * FROM schools S
                                        WHERE 
                                            (SELECT SUM(T.minutes) FROM tasks T WHERE T.userId IN (SELECT UP.id
                                            FROM userprofile UP WHERE UP.schoolid = S.id)) <> '' 
                                                GROUP BY S.name  ";

            List<School> schoolResponse = new List<School>();

            DataTable dataTable = sqlTools.GetTable(schoolQuery);

            if (dataTable != null)
            {
                schoolResponse = sqlTools.ConvertDataTable<School>(dataTable);
            }

            //Get List of Faculty Ids
            string facultyQuery = @"SELECT * FROM faculties F
                                        WHERE 
                                            (SELECT SUM(T.minutes) FROM tasks T WHERE T.userId IN (SELECT UP.id
                                            FROM userprofile UP WHERE UP.facultyid = F.id)) <> '' 
                                                GROUP BY F.name ";

            List<Faculty> facultyResponse = new List<Faculty>();

            dataTable = sqlTools.GetTable(facultyQuery);

            if (dataTable != null)
            {
                facultyResponse = sqlTools.ConvertDataTable<Faculty>(dataTable);
            }


            //FILL RESPONSE
            foreach (School school in schoolResponse)
            {
                foreach (Faculty faculty in facultyResponse)
                {
                    string query = @"SELECT CONCAT((SELECT name FROM schools WHERE id = @SchoolId), ' ',  F.name) AS `name1`, 
                                    IFNULL(CAST((SELECT ROUND(SUM(T.minutes),0) FROM tasks T WHERE T.userId IN (SELECT UP.id
                                     FROM userprofile UP WHERE UP.facultyid = @FacultyId AND UP.schoolid = @SchoolId)) AS double),0) AS `value1` 
                                        FROM faculties F WHERE id = @FacultyId
                                        GROUP BY F.name";


                    //Will only return 1 row
                    DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object> { { "@FacultyId", faculty.Id }, { "@SchoolId", school.Id } });

                    if ( dataRow != null )
                    {
                        BaseFilterResponseItem returnRow = new BaseFilterResponseItem();

                        PropertyInfo[] properties = typeof(BaseFilterResponseItem).GetProperties();

                        int i = 0;
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.Name == "Name1" || property.Name == "Value1")
                            {
                                property.SetValue(returnRow, dataRow.ItemArray[i]);
                                i++;
                            }
                        }

                        if (returnRow.Value1 > 0)
                        {
                            filterResponse.ResponseItems.Add(returnRow);
                        }
                    }
                }
            }

            return filterResponse;
        }
    }
}
