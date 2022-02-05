using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Personals.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using BaseFilterResponse = plannerBackEnd.Common.Filters.DomainObjects.BaseFilterResponse;

namespace plannerBackEnd.Personals.DataAccess.Dao
{
    public class PersonalChartsDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public PersonalChartsDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListSubjectTotalHours(BaseFilterRequest filter)
        {

            BaseFilterResponse filterResponse = new BaseFilterResponse();

            string query = @"SELECT CONCAT(S.name, ' ', S.classcode) AS `name1`, 
                                    S.color AS `name2`,
                                IFNULL(CAST((SELECT ROUND(SUM(minutes),0) FROM tasksessions WHERE taskId IN (SELECT id FROM tasks WHERE subjectId = S.id)) 
                                    AS double),0) AS `value1`
                                FROM subjects S WHERE userid = @UserId AND  
                                (SELECT SUM(minutes) FROM tasksessions WHERE taskId IN (SELECT id FROM tasks WHERE subjectId = S.id)) > 0 AND semesterid = @semesterId";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { 
            { "@UserId", filter.UserId},
            { "SemesterId", filter.SemesterId } });

            if (dataTable != null)
            {

                filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListSubjectBreakdown(BaseFilterRequest filter)
        {

            List<BaseFilterResponse> filterResponse = new List<BaseFilterResponse>();

            BaseFilterResponse temporaryResponse = new BaseFilterResponse();

            string query = @"SELECT tasktype AS `name1`, SUM(minutes) AS `value1`, subjectid AS `value2` FROM tasks WHERE userid = @UserId
                            AND minutes > 0 GROUP BY subjectid, tasktype ORDER BY subjectid;  ";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

            if (dataTable != null)
            {
                temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);

                for (int i = 0; i < temporaryResponse.ResponseItems.Count; i++)
                {
                    if (i == 0)
                    {
                        filterResponse.Add(new BaseFilterResponse()
                        {
                            Title = temporaryResponse.ResponseItems[i].Value2.ToString(),
                            ResponseItems = new List<BaseFilterResponseItem>{temporaryResponse.ResponseItems[i]}
                        });
                    }
                    else if (temporaryResponse.ResponseItems[i].Value2 != temporaryResponse.ResponseItems[i - 1].Value2)
                    {
                        filterResponse.Add(new BaseFilterResponse()
                        {
                            Title = temporaryResponse.ResponseItems[i].Value2.ToString(),
                            ResponseItems = new List<BaseFilterResponseItem> { temporaryResponse.ResponseItems[i] }
                        });
                    }
                    else if (temporaryResponse.ResponseItems[i].Value2 == temporaryResponse.ResponseItems[i - 1].Value2)
                    {
                        filterResponse.Find(x => x.Title == temporaryResponse.ResponseItems[i].Value2.ToString()).ResponseItems.Add(temporaryResponse.ResponseItems[i]);
                    }
                }
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListHoursPerWeek(BaseFilterRequest filter)
        {
            List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            BaseFilterResponse filterResponse = new BaseFilterResponse();
            for (int i = 0; i < daysOfWeek.Count; i++)
            {
                filterResponse.ResponseItems.Add(new BaseFilterResponseItem() { Name1 = daysOfWeek[i] });
            }



            string query = @"SELECT WEEKDAY(datecompleted) AS `value1`,
                                 CAST((SELECT ROUND(SUM(minutes),0)) AS double) AS `value2`
                                 FROM tasksessions WHERE YEARWEEK(@Date, 1) = YEARWEEK(dateCompleted, 1)  
                                 AND taskId IN(SELECT id FROM tasks WHERE userid = @userId) GROUP BY dateCompleted ORDER BY dateCompleted";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                { "@Date", filter.Date }
            });

            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse1 = new BaseFilterResponse();
                temporaryResponse1.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                for (int i = 0; i < temporaryResponse1.ResponseItems.Count; i++)
                {
                    filterResponse.ResponseItems[(int) temporaryResponse1.ResponseItems[i].Value1].Value1 = temporaryResponse1.ResponseItems[i].Value2;
                }
            }

            ///TO generate Streak
            query = @"SELECT TS.datecompleted AS `date1`
                        FROM tasksessions TS LEFT JOIN tasks T ON T.id = TS.taskid
                        WHERE TS.minutes > 0 AND userid = @UserId
                            GROUP BY TS.datecompleted, T.userid
                                ORDER BY T.userid, TS.datecompleted DESC ";

            dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId }
            });

            int streak = 0;

            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse = new BaseFilterResponse();
                temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                for (int i = 0; i < temporaryResponse.ResponseItems.Count; i++)
                {
                    if (i == 0 && (temporaryResponse.ResponseItems[i].Date1 - DateTime.Today).TotalDays <= 1)
                    {
                        streak++;
                    }
                    else if (i == 0 && (temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i + 1].Date1).TotalDays > 1 )
                    {
                        break;
                    }
                    else if ((temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i - 1].Date1).TotalDays >= -1)
                    {
                        streak++;
                    }
                    else if ((temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i - 1].Date1).TotalDays < -1)
                    {
                        break;
                    }
                }
            }

            filterResponse.Title = streak.ToString();
            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListHoursPerMonth(BaseFilterRequest filter)
        {

            List<BaseFilterResponse> filterResponse = new List<BaseFilterResponse>();

            string query = @"SELECT STR_TO_DATE(CONCAT(YEARWEEK(DATE_SUB(datecompleted, INTERVAL 1 DAY)), ' Monday'), '%X%V %W') AS `date1`,
                                (SELECT color FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id)) AS `name1`,
                                (SELECT CONCAT(name, ' ', classcode) FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id)) AS `name2`,
                                    IFNULL(CAST((SELECT ROUND(SUM(minutes),0)) AS double),0) AS `value1` FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @userId) 
                                    AND (SELECT semesterid FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id)) = @SemesterId
                                    GROUP BY datecompleted, 
                                        (SELECT name FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id)),
                                        (SELECT classcode FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id))
                                    ORDER BY 
                                        (SELECT CONCAT(name, ' ', classcode) FROM subjects WHERE id = (SELECT subjectid FROM tasks WHERE taskid = id)),YEARWEEK(dateCompleted) ASC";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                {"SemesterId", filter.SemesterId }
            });

            if (dataTable != null)
            {
                if (dataTable.Rows.Count != 0)
                {
                    BaseFilterResponse temporaryResponse = new BaseFilterResponse();
                    temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                    for (int i = 0; i < temporaryResponse.ResponseItems.Count; i++)
                    {
                        if (i == 0)
                        {
                            filterResponse.Add(new BaseFilterResponse() {Title = temporaryResponse.ResponseItems[i].Name2, Color = temporaryResponse.ResponseItems[i].Name1});
                        }
                        else if (temporaryResponse.ResponseItems[i].Name2 != temporaryResponse.ResponseItems[i - 1].Name2)
                        {
                            filterResponse.Add(new BaseFilterResponse() {Title = temporaryResponse.ResponseItems[i].Name2, Color = temporaryResponse.ResponseItems[i].Name1});
                        }
                    }

                    List<DateTime> mondaysFromStartToNow = new List<DateTime>();
                    DateTime temporaryDate = temporaryResponse.ResponseItems[0].Date1;

                    foreach (BaseFilterResponseItem item in temporaryResponse.ResponseItems)
                    {
                        if (DateTime.Compare(item.Date1, temporaryDate) < 0)
                        {
                            temporaryDate = item.Date1;
                        }
                    }

                    while (DateTime.Now > temporaryDate)
                    {
                        mondaysFromStartToNow.Add(temporaryDate);
                        temporaryDate = temporaryDate.AddDays(7);
                    }

                    foreach (BaseFilterResponse responseItem in filterResponse)
                    {
                        foreach (DateTime mondayDay in mondaysFromStartToNow)
                        {
                            responseItem.ResponseItems.Add(new BaseFilterResponseItem() {Name1 = mondayDay.ToString()});

                            foreach (BaseFilterResponseItem item in temporaryResponse.ResponseItems)
                            {
                                if (responseItem.Title == item.Name2 && item.Date1 == mondayDay)
                                {
                                    responseItem.ResponseItems.Find(x => x.Name1.Contains(mondayDay.ToString())).Value1 += item.Value1;
                                }
                            }
                        }

                    }
                }
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListAverageHoursPerWeek(BaseFilterRequest filter)
        {
            List<string> daysOfWeek = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            BaseFilterResponse filterResponse = new BaseFilterResponse();
            for (int i = 0; i < daysOfWeek.Count; i++)
            {
                filterResponse.ResponseItems.Add(new BaseFilterResponseItem() { Name1 = daysOfWeek[i] });
            }



            string query = @"SELECT WEEKDAY(TS.datecompleted) AS `value1` ,
                                SUM(TS.minutes)/((DATEDIFF(STR_TO_DATE(CONCAT(YEARWEEK(@Date), ' Monday'), '%X%V %W'),
                                    (STR_TO_DATE(CONCAT(YEARWEEK(UP.rowcreated), ' Monday'), '%X%V %W'))))/7) AS `value2`
                                         FROM tasksessions TS 
                                        LEFT JOIN tasks T ON T.id = TS.taskid 
                                        LEFT JOIN userprofile UP ON UP.id = T.userid
                                            WHERE T.userid = @UserId AND TS.datecompleted > UP.rowcreated GROUP BY WEEKDAY(TS.datecompleted) ORDER BY WEEKDAY(TS.datecompleted) ASC";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                { "@Date", filter.Date }
            });


            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse1 = new BaseFilterResponse();
                temporaryResponse1.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                for (int i = 0; i < temporaryResponse1.ResponseItems.Count; i++)
                {
                    filterResponse.ResponseItems[(int)temporaryResponse1.ResponseItems[i].Value1].Value1 = temporaryResponse1.ResponseItems[i].Value2;
                }
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public PersonalStatisticsEntity GetListPersonalStats(BaseFilterRequest filter)
        {
            string query = @"SELECT
                                IFNULL((SELECT Coalesce(CAST(ROUND(SUM(minutes),0) AS double),0) 
                                    FROM tasksessions WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) AND 
                                    datecompleted = @Date GROUP BY datecompleted),0) AS `currentday`,

                                IFNULL((SELECT IFNULL(CAST((SELECT ROUND(SUM(minutes),0)) AS double),0) FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) AND YEARWEEK(dateCompleted,1) = YEARWEEK(@Date,1)
                                    GROUP BY YEARWEEK(datecompleted,1)),0) AS `currentweek`,
 
                                IFNULL((SELECT
                                    IFNULL(CAST((SELECT ROUND(SUM(minutes),0)) AS double),0) FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) AND MONTH(dateCompleted) = MONTH(@Date)
                                    GROUP BY MONTH(datecompleted)),0) AS `currentmonth`,
 
                                (SELECT
                                    IFNULL(CAST(ROUND(SUM(minutes)/DATEDIFF(@Date,(SELECT rowcreated FROM userprofile WHERE id = @UserId)),0) AS double),0)
                                    AS `Value1` FROM tasksessions WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId)) AS `averageday`,
         
                                IFNULL((SELECT AVG(`value1`) AS `value1` FROM 
                                    (SELECT CAST(ROUND(SUM(minutes),0) AS double) AS `value1` FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) 
                                    GROUP BY YEARWEEK(datecompleted) ORDER BY value1 DESC) AS v),0) AS `averageweek`,

                                IFNULL((SELECT avg(`Value1`) AS `Value1` FROM 
                                    (SELECT IFNULL(CAST((SELECT ROUND(SUM(minutes),0)) AS double),0) AS `Value1` FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId)
                                    GROUP BY MONTH(datecompleted)) AS t),0) AS `averagemonth`,

                                IFNULL((SELECT MAX(`value1`) AS `value1` 
                                    FROM (SELECT CAST(ROUND(SUM(minutes),0) AS double) AS `value1`  FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) 
                                    GROUP BY datecompleted ORDER BY value1 DESC) as t),0) AS `bestday`,
 
                                IFNULL((SELECT MAX(`value1`) AS `value1` FROM 
                                    (SELECT CAST(ROUND(SUM(minutes),0) AS double) AS `value1` FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) 
                                    GROUP BY YEARWEEK(datecompleted) ORDER BY value1 DESC) AS s),0) AS `bestweek`,

                                IFNULL((SELECT MAX(`value1`) AS `value1` FROM 
                                    (SELECT CAST(ROUND(SUM(minutes),0) AS double) AS `value1` FROM tasksessions 
                                    WHERE taskId IN (SELECT id FROM tasks WHERE userid = @UserId) 
                                    GROUP BY MONTH(datecompleted) ORDER BY value1 DESC) AS u),0) AS `bestmonth`

                                    FROM tasksessions GROUP BY `currentmonth`  ";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                { "@Date", filter.Date }
            });

            PersonalStatisticsEntity returnRow = new PersonalStatisticsEntity();

            PropertyInfo[] properties = typeof(PersonalStatisticsEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<DetailedViewEntity> GetListDetailedView(BaseFilterRequest filter)
        { 
            List<DetailedViewEntity> returnArray = new List<DetailedViewEntity>();
            string query = @"SELECT T.tasktype, T.title, T.description, 
                                CONCAT(S.name, ' ', S.classcode) AS 'subject' , 
                                S.color AS 'subjectcolor', 
                                S.description AS 'subjectdescription', 
                                T.minutes AS `totalminutes`,
                                SUM(TS.minutes) AS `sessionminutes`, TS.datecompleted, T.duedate,
                                DATEDIFF(TS.datecompleted, T.duedate) AS `datedifference`, TS.minutes/T.minutes AS `minutepercentage`
                                    FROM tasksessions TS 
                                        LEFT JOIN tasks T ON T.id = TS.taskid
                                        LEFT JOIN subjects S ON S.id = T.subjectid
                                 WHERE TS.taskid IN (SELECT T.id FROM tasks T WHERE userid = @userId AND isDone = 1 ";

            if (filter.Tasktype.Length > 0) { 
                query += " AND T.tasktype = @tasktype ";
            }

            if (filter.SubjectId > 0)
            {
                query += " AND S.id = @subjectId ";
            }
            query += ") GROUP BY T.title, CONCAT(S.name, ' ', S.classcode) , TS.datecompleted ORDER BY T.minutes DESC, T.title, DATEDIFF(TS.datecompleted, T.duedate) ";
            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                { "@tasktype", filter.Tasktype },
                { "@subjectId", filter.SubjectId }
            });

            List<DetailedViewEntity> temporaryResponse1 = new List<DetailedViewEntity>();
            if (dataTable != null)
            {
                temporaryResponse1 = sqlTools.ConvertDataTable<DetailedViewEntity>(dataTable);
            }

            for (int i = 0 ;  i < temporaryResponse1.Count; i++)
            {
                if (i == 0)
                {
                    temporaryResponse1[i].SessionItems = new List<DetailedViewItemEntity>();
                    temporaryResponse1[i].SessionItems.Add(new DetailedViewItemEntity()
                    {
                        DateCompleted = temporaryResponse1[i].DateCompleted,
                        DateDifference = temporaryResponse1[i].DateDifference,
                        SessionMinutes = temporaryResponse1[i].SessionMinutes,
                        MinutePercentage = temporaryResponse1[i].MinutePercentage,
                    });
                    returnArray.Add(temporaryResponse1[i]);
                }
                else if (temporaryResponse1[i].Title == temporaryResponse1[i - 1].Title && temporaryResponse1[i].Subject == temporaryResponse1[i - 1].Subject)
                {
                    returnArray.Find(x => x.Title == temporaryResponse1[i].Title && x.Subject == temporaryResponse1[i].Subject).SessionItems.Add(new DetailedViewItemEntity()
                        {
                            DateCompleted = temporaryResponse1[i].DateCompleted,
                            DateDifference = temporaryResponse1[i].DateDifference,
                            SessionMinutes = temporaryResponse1[i].SessionMinutes,
                            MinutePercentage = temporaryResponse1[i].MinutePercentage,
                        });
                }
                else if (temporaryResponse1[i].Title != temporaryResponse1[i - 1].Title || temporaryResponse1[i].Subject != temporaryResponse1[i - 1].Subject)
                {
                    temporaryResponse1[i].SessionItems = new List<DetailedViewItemEntity>();
                    temporaryResponse1[i].SessionItems.Add(new DetailedViewItemEntity()
                    {
                        DateCompleted = temporaryResponse1[i].DateCompleted,
                        DateDifference = temporaryResponse1[i].DateDifference,
                        SessionMinutes = temporaryResponse1[i].SessionMinutes,
                        MinutePercentage = temporaryResponse1[i].MinutePercentage,
                    });
                    returnArray.Add(temporaryResponse1[i]);
                }
            }

            
            return returnArray;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListCalendarView(BaseFilterRequest filter)
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            string query = @"SELECT SUM(minutes) AS `value1`, datecompleted AS `date1` FROM dbplanner.tasksessions 
                                WHERE (SELECT userid FROM tasks T WHERE T.id = taskid) = @UserId
                                    GROUP BY datecompleted ORDER BY datecompleted ASC; ";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

            if (dataTable != null)
            {
                filterResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
            }

            return filterResponse;
        }
    }
}
