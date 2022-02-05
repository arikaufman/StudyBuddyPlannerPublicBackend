using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Feeds.DataAccess.Entities;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace plannerBackEnd.Feeds.DataAccess.Dao
{
    public class FeedDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public FeedDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public FeedEntity Get(int requestedId)
        {
            string query = "SELECT * FROM feeds WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            FeedEntity returnRow = new FeedEntity();

            PropertyInfo[] properties = typeof(FeedEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------

        public FeedEntity GetReferenceId(FeedFilterRequest filter)
        {
            string query = "SELECT * FROM dbplanner.feeds WHERE displaytype = @DisplayType and referenceId = @referenceId ";

            if (filter.DisplayType == "streak")
            {
                query += " and rowcreated < @Date";
            }
            else
            {
                query += " and DATEDIFF(rowcreated, @Date) = 0";
            }
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object> {

                { "@referenceId", filter.ReferenceId},
                { "@DisplayType", filter.DisplayType},
                { "@Date", filter.CurrentTime}
             });


            FeedEntity returnRow = new FeedEntity();

            if (dataRow != null)
            {
                PropertyInfo[] properties = typeof(FeedEntity).GetProperties();

                int i = 0;
                foreach (PropertyInfo property in properties)
                {
                    property.SetValue(returnRow, dataRow.ItemArray[i]);
                    i++;
                }
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<FeedEntity> GetList(FeedFilterRequest filter)
        {

            List<FeedEntity> filterResponse = new List<FeedEntity>();

            string query = @"SELECT * FROM feeds WHERE userId = @UserId 
                    OR (userId IN (SELECT userid1 FROM friends WHERE userid2 = @UserId AND status = 2) AND (SELECT feedprivacy FROM userprofile UP WHERE UP.id = userid) = 0)
                    OR (userId IN (SELECT userid2 FROM friends WHERE userid1 = @UserId AND status = 2) AND (SELECT feedprivacy FROM userprofile UP WHERE UP.id = userid) = 0)
                    OR visibility = 0 ORDER BY rowcreated DESC LIMIT @Limit;";


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>
            {
                { "@UserId", filter.UserId },
                { "@Limit", filter.Limit }
            });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<FeedEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public FeedEntity Create(FeedEntity feedEntity)
        {
            string query = "";
            if (feedEntity.DisplayType == "tasksession")
            {
                query = TaskSessionString();
            }
            else if (feedEntity.DisplayType == "taskcompleted")
            {
                query = TaskCompletedString();
            }
            else if (feedEntity.DisplayType == "streak")
            {
                query = StreakString();
            }
            else if (feedEntity.DisplayType == "fiveHoursSpent")
            {
                query = FiveHoursSpentString();
            }
            else if (feedEntity.DisplayType == "friendAccept")
            {
                query = FriendAcceptString();
            }
            else if (feedEntity.DisplayType == "bestDay")
            {
                query = BestDayString();
            }

            query += "SELECT * FROM feeds WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", feedEntity.UserId},
                {"@selfdescription", feedEntity.SelfDescription},
                {"@generaldescription", feedEntity.GeneralDescription},
                {"@visibility", feedEntity.Visibility},
                {"@displaytype", feedEntity.DisplayType},
                {"@referenceId", feedEntity.ReferenceId},
            });

            FeedEntity returnRow = new FeedEntity();
            PropertyInfo[] properties = typeof(FeedEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public FeedEntity Update(FeedEntity feedEntity)
        {
            string query = @"UPDATE feeds SET 
                            userid = @userid,
                            selfdescription = @selfdescription,
                            generaldescription = @generaldescription,
                            visibility = @visibility,
                            displaytype = @displaytype,
                            referenceId = @referenceId
                            WHERE id = @id; 

                SELECT * FROM feeds WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", feedEntity.UserId},
                {"@selfdescription", feedEntity.SelfDescription},
                {"@generaldescription", feedEntity.GeneralDescription},
                {"@visibility", feedEntity.Visibility},
                {"@displaytype", feedEntity.DisplayType},
                {"@referenceId", feedEntity.ReferenceId},
            });

            FeedEntity returnRow = new FeedEntity();
            PropertyInfo[] properties = typeof(FeedEntity).GetProperties();
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
            string query = "DELETE FROM feeds WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }

        // --------------------------------------------------------------------------------------------
        public bool DeleteReferenceItem(int referenceId, string displayType)
        {
            string query = "DELETE FROM feeds WHERE referenceId = @id AND displaytype = @displaytype";

            int result = sqlTools.Execute(query, new Dictionary<string, object>
            {
                { "@id", referenceId },
                { "@displaytype", displayType }
            });

            return (result > 0 ? true : false);
        }

        // --------------------------------------------------------------------------------------------
        private string TaskSessionString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT
                            @userId,
                            CONCAT('You completed a ', FLOOR(TS.minutes/60),
                            'hrs., ', MOD(TS.minutes, 60), 'min. long study session for ', CONCAT(S.name, ' ', S.classcode), '.') ,
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname), ' completed a ', FLOOR(TS.minutes/60),
                            'hrs., ', MOD(TS.minutes, 60), 'min. long study session for ', CONCAT(S.name, ' ', S.classcode), '.') ,
                            @visibility,
                            @displaytype,
                            @referenceId
                            FROM tasksessions TS
                            LEFT JOIN tasks T ON T.id = TS.taskid
                            LEFT JOIN subjects S ON S.id = T.subjectid
                            LEFT JOIN userprofile UP ON S.userid = UP.id
                            WHERE TS.id = @referenceId;";
        }

        // --------------------------------------------------------------------------------------------
        private string TaskCompletedString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT 
                            @userId,
                            CONCAT('You completed a ', CONCAT(S.name, ' ', S.classcode), ' ', T.tasktype, 
                            ' that took ', FLOOR(T.minutes/60),
                            'hrs., ', MOD(T.minutes, 60), 'min!') ,
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname), ' completed a ', CONCAT(S.name, ' ', S.classcode), ' ', T.tasktype, 
                            ' that took ', FLOOR(T.minutes/60),
                            'hrs., ', MOD(T.minutes, 60), 'min!') ,
                            @visibility,
                            @displaytype,
                            @referenceId
                            FROM tasks T
                            LEFT JOIN subjects S ON S.id = T.subjectid
                            LEFT JOIN userprofile UP ON S.userid = UP.id
                            WHERE T.id = @referenceId; ";
        }

        // --------------------------------------------------------------------------------------------
        private string StreakString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT 
                            @userId,
                            CONCAT('You just reached a ', @generalDescription, '  day study streak ! '),
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname),
                            ' just reached a ', @generalDescription, ' day study streak ! ') ,
                            @visibility,
                            @displaytype,
                            @referenceId
                            FROM userprofile UP WHERE UP.id = @userId;  ";
        }

        // --------------------------------------------------------------------------------------------
        private string FiveHoursSpentString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT 
                            @userId,
                            CONCAT('You studied for ', @generalDescription, ' today. Congratulations ! Make sure you take a good study break. '),
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname), ' studied for ', @generalDescription , ' today ! ') ,
                            @visibility,
                            @displayType,
                            @referenceId
                            FROM userprofile UP WHERE UP.id = @userId; ";
        }

        // --------------------------------------------------------------------------------------------
        private string FriendAcceptString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT 
                            F.userid1,
                            CONCAT(CONCAT(UP2.firstname, ' ', UP2.lastname), ' is now your friend! '),
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname), ' and ' , CONCAT(UP2.firstname, ' ', UP2.lastname), ' are now friends! ') ,
                            @visibility,
                            @displayType,
                            @referenceId
                            FROM friends F
                            LEFT JOIN userprofile UP ON UP.id = F.userid1
                            LEFT JOIN userprofile UP2 on UP2.id = F.userid2 WHERE F.id = @referenceId; ";
        }

        // --------------------------------------------------------------------------------------------
        private string BestDayString()
        {
            return @"INSERT INTO feeds (
                            userid,
                            selfdescription,
                            generaldescription,
                            visibility,
                            displaytype,
                            referenceId) 

                            SELECT 
                            @UserId,
                            CONCAT(' You just finished a study day that put you in your top 5 biggest days among your friends. '),
                            CONCAT(CONCAT(UP.firstname, ' ', UP.lastname), ' just finished a huge study day that put them in your friends top 5 biggest days. ', 
                            'They spent ', @generalDescription , ' studying today!') ,
                            @visibility,
                            @displayType,
                            @referenceId
                            FROM userprofile UP WHERE UP.id = @userId; ";
        }
    }
}