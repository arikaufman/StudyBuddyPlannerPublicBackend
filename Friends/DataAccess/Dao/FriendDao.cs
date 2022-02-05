using System;
using Common.Enums;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Friends.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Friends.Domain.DomainObjects;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Friends.DataAccess.Dao
{
    public class FriendDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public FriendDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // -----------------------------------------------------------------------------

        public List<FriendEntity> GetListFriends(FriendFilterRequest friendFilterRequest)
        {

            List<FriendEntity> filterResponse = new List<FriendEntity>();

            string query = "SELECT * FROM friends WHERE (userid1 = @userId OR userid2 = @userId)  ";

            if (friendFilterRequest.Pending)
            {
                query += " AND status = 1";
            }

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@userId", friendFilterRequest.Id } });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<FriendEntity>(dataTable);
            }

            return filterResponse;
        }

         // -----------------------------------------------------------------------------

         public List<ActiveFriendEntity> GetListActiveFriends(ActiveFriendFilterRequest activeFriendFilterRequest)
         {
             List<ActiveFriendEntity> filterResponse = new List<ActiveFriendEntity>();

             string query = @"SELECT UP.id, UP.firstname, UP.lastname,
                                IF((SELECT id FROM friends WHERE userid1 = @userId AND status = 2 AND userid2 = UP.id) > 0,
                                (SELECT id FROM friends WHERE userid1 = @userId AND status = 2 AND userid2 = UP.id),
                                (SELECT id FROM friends WHERE userid2 = @userId AND status = 2 AND userid1 = UP.id)) AS `friendrowid`,

                                IFNULL((SELECT name FROM schools WHERE UP.schoolid = id),'') AS 'school',
                                IFNULL((SELECT name FROM faculties WHERE UP.facultyid = id),'') AS 'faculty',
                                IFNULL(UP.major,'') AS `major`,
                                IFNULL(S.name,'') AS 'subjectname',
                                IFNULL(S.classcode,'') AS 'subjectclasscode',
                                IFNULL(T.tasktype,'') AS `tasktype`,
                                IFNULL(T.title,'') AS 'taskdescription',
                                IFNULL(UA.lastactive, UTC_timestamp) AS `lastactive`,
                                IFNULL(UA.active, 0) AS `active`,
                                IFNULL(CAST((SELECT ROUND(UA.timezoneoffset,0)) AS double),0) AS `timezoneoffset`
                                     FROM userprofile UP

                                     LEFT JOIN useractivity UA ON UA.userid = UP.id
                                     LEFT JOIN tasks T ON T.id = UA.currenttaskid
                                     LEFT JOIN subjects S ON S.id = T.subjectid
                                        WHERE
                                (UP.id IN(SELECT userid1 FROM friends WHERE userid2 = @userId AND status = 2) OR
                                UP.id IN(SELECT userid2 FROM friends WHERE userid1 = @userId AND status = 2)) 
                                ORDER BY UA.active DESC, UA.lastactive DESC; ";


             DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@userId", activeFriendFilterRequest.Id } });

             if (dataTable != null)
             {
                 filterResponse = sqlTools.ConvertDataTable<ActiveFriendEntity>(dataTable);
             }

             return filterResponse;
         }

        // -----------------------------------------------------------------------------

        public List<SuggestedFriendEntity> GetListSuggestedFriends(UserProfile userProfile)
        {
            List<SuggestedFriendEntity> filterResponse = new List<SuggestedFriendEntity>();

            string query = @"SELECT 
                                UP.id,
                                UP.firstname AS 'firstname',
                                UP.lastname AS 'lastname', 
                                (SELECT name FROM schools S WHERE S.id = UP.schoolid) AS 'school', 
                                (SELECT name FROM faculties F WHERE F.id = UP.facultyid) AS 'faculty',
                                (SELECT COUNT(*) FROM friends F WHERE userid1 = UP.id OR userid2 = UP.id) AS 'numberOfFriends',
                                (SELECT active FROM useractivity UA WHERE UA.userid = UP.id) AS 'active'

                                FROM userprofile UP
                                    WHERE UP.id NOT IN (SELECT userid1 FROM friends WHERE userid2 = @userId) AND 
                                    UP.id NOT IN (SELECT userid2 FROM friends WHERE userid1 = @userId)
                                        AND UP.major != 'fakeuser' AND UP.id != @userId AND 
                                        DATEDIFF(CURDATE(),(SELECT UA.lastactive FROM useractivity UA WHERE UA.userid = UP.id)) <= 7
                                    ORDER BY 
                                    UP.schoolid = @schoolId DESC, 
                                    UP.facultyid = @facultyId DESC, 
                                        (SELECT COUNT(*) FROM friends F WHERE userid1 = UP.id OR userid2 = UP.id) DESC,
                                        UP.year = @year DESC LIMIT 15
";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@userId", userProfile.Id },
                { "@schoolId", userProfile.SchoolId },
                { "@facultyId", userProfile.FacultyId },
                { "@year", userProfile.Year }
            });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<SuggestedFriendEntity>(dataTable);
            }

            return filterResponse;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListFriendStreaks (BaseFilterRequest request)
        {
            BaseFilterResponse filterResponse = new BaseFilterResponse();

            ///TO generate Streak
            string query = @"SELECT (SELECT CONCAT(firstname, ' ', lastname) FROM userprofile WHERE id = T.userid) AS `name1`,
                            TS.datecompleted AS `date1`, T.userid AS `value2`
                            FROM tasksessions TS LEFT JOIN tasks T ON T.id = TS.taskid
                        WHERE TS.minutes > 0 AND userid IN 
                        (SELECT userid1 FROM friends WHERE userid2 = @UserId AND status = 2) OR userid IN 
                        (SELECT userid2 FROM friends WHERE userid1 = @UserId AND status = 2) OR userid = @UserId
                            GROUP BY TS.datecompleted, T.userid
                                ORDER BY T.userid, TS.datecompleted DESC  ";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", request.UserId }
            });

            if (dataTable != null)
            {
                BaseFilterResponse temporaryResponse = new BaseFilterResponse();
                temporaryResponse.ResponseItems = sqlTools.ConvertDataTable<BaseFilterResponseItem>(dataTable);
                List<BaseFilterResponse> temporaryResponses = new List<BaseFilterResponse>();
                for (int i = 0; i < temporaryResponse.ResponseItems.Count; i++)
                {
                    if (i == 0)
                    {
                        temporaryResponses.Add(new BaseFilterResponse() { Title = temporaryResponse.ResponseItems[i].Value2.ToString() });
                    }
                    else if (temporaryResponse.ResponseItems[i].Value2 != temporaryResponse.ResponseItems[i - 1].Value2)
                    {
                        temporaryResponses.Add(new BaseFilterResponse() { Title = temporaryResponse.ResponseItems[i].Value2.ToString() });
                    }

                }

                filterResponse.ResponseItems.Add(temporaryResponse.ResponseItems[0]);
                foreach (BaseFilterResponse baseFilterResponse in temporaryResponses)
                {
                    for (int i = temporaryResponse.ResponseItems.FindIndex(x => x.Value2.ToString() == baseFilterResponse.Title); i < temporaryResponse.ResponseItems.Count; i++)
                    {

                        if (i == temporaryResponse.ResponseItems.FindIndex(x => x.Value2.ToString() == baseFilterResponse.Title)
                            && (DateTime.Today - temporaryResponse.ResponseItems[i].Date1).TotalDays >= 0 && (DateTime.Today - temporaryResponse.ResponseItems[i].Date1).TotalDays <= 1)
                        {
                            if (i == 0)
                            {
                                temporaryResponse.ResponseItems[i].Value1 += 1;
                            }
                            else
                            {
                                temporaryResponse.ResponseItems[i].Value1 += 1;
                                filterResponse.ResponseItems.Add(temporaryResponse.ResponseItems[i]);
                            }
                        }
                        else if (i == temporaryResponse.ResponseItems.FindIndex(x => x.Value2.ToString() == baseFilterResponse.Title) 
                                 && (temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i + 1].Date1).TotalDays > 1)
                        {
                            break;
                        }
                        else if (!filterResponse.ResponseItems.Exists(x => x.Value2.ToString() == baseFilterResponse.Title))
                        {
                            break;
                        }
                        else if ((temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i - 1].Date1).TotalDays >= -1)
                        {
                            filterResponse.ResponseItems.Find(x => x.Value2.ToString() == baseFilterResponse.Title).Value1 += 1;
                        } 
                        else if ((temporaryResponse.ResponseItems[i].Date1 - temporaryResponse.ResponseItems[i - 1].Date1).TotalDays < -1)
                        {
                            break;
                        }
                    }
                }
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public FriendEntity SendRequest(FriendEntity friendEntity)
        {
            string query = @"INSERT INTO friends (
                            userid1,
                            userid2,
                            status) 

                            VALUES(
                            @userid1,
                            @userid2,
                            @status);  ";

            query += "SELECT * FROM friends WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid1", friendEntity.UserId1},
                {"@userid2", friendEntity.UserId2},
                {"@status", FriendStatus.Pending},
            });

            FriendEntity returnRow = new FriendEntity();
            PropertyInfo[] properties = typeof(FriendEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public FriendEntity AcceptRequest(int requestedId)
        {
            string query = @"UPDATE friends SET 
                            status = @status
                            WHERE id = @id; 

                SELECT * FROM friends WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@status", FriendStatus.Accepted},
                {"@id", requestedId}
            });

            FriendEntity returnRow = new FriendEntity();
            PropertyInfo[] properties = typeof(FriendEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }
            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public FriendEntity DeclineRequest(int requestedId)
        {
            string query = @"UPDATE friends SET 
                            status = @status
                            WHERE id = @id; 

                SELECT * FROM friends WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@status", FriendStatus.None},
                {"@id", requestedId}
            });

            FriendEntity returnRow = new FriendEntity();
            PropertyInfo[] properties = typeof(FriendEntity).GetProperties();
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
            string query = "DELETE FROM friends WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }
    }
}