using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Users.DataAccess.Entities;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess.Dao
{
    public class UserActivityDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public UserActivityDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public UserActivityEntity Get(int requestedId)
        {
            string query = "SELECT * FROM useractivity WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            UserActivityEntity returnRow = new UserActivityEntity();

            PropertyInfo[] properties = typeof(UserActivityEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public Dictionary<string, int> GetCount(UserActivityFilterRequest filter)
        {
            Dictionary<string,int> returnDict = new Dictionary<string, int>();

            string query = "SELECT COUNT(*) FROM useractivity WHERE active = 1";
            int countWorldCurUsers = Convert.ToInt32(sqlTools.ExecuteScalar(query, new Dictionary<string, object> { }));
            returnDict.Add("WorldCurUsers", countWorldCurUsers );

            string query2 = "SELECT COUNT(*) FROM useractivity WHERE active = 1 AND userid IN (SELECT id FROM userprofile WHERE schoolid = @schoolid)";
            int countSchoolCurUsers = Convert.ToInt32(sqlTools.ExecuteScalar(query2, new Dictionary<string, object> {
                {"@schoolid", filter.School} }));
            returnDict.Add("SchoolCurUsers", countSchoolCurUsers);

            string query3 = "SELECT COUNT(*) FROM useractivity WHERE active = 1 AND userid IN (SELECT id FROM userprofile WHERE schoolid = @schoolid AND facultyid = @facultyid)";
            int countSchoolFacultyCurUsers = Convert.ToInt32(sqlTools.ExecuteScalar(query3, new Dictionary<string, object> {
                {"@schoolid", filter.School},
                {"@facultyid", filter.Faculty }
            }));
            returnDict.Add("SchoolFacultyCurUsers", countSchoolFacultyCurUsers);

            return returnDict;
        }

        //----------------------------------------------------------------------------------------------------
        public UserActivityEntity Create(UserActivityEntity userActivityEntity)
        {
            string query = @"INSERT INTO useractivity (
                            userid,
                            active,
                            currenttaskid,
                            lastactive,
                            timezoneoffset) 

                            VALUES(
                            @userid,
                            @active,
                            @currenttaskid,
                            @lastactive,
                            @timezoneoffset) ON DUPLICATE KEY UPDATE active = @active, currenttaskid = @currenttaskid, lastactive = @lastactive; ";

            query += "SELECT * FROM useractivity WHERE userid = @userid";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", userActivityEntity.UserId},
                {"@active", userActivityEntity.Active},
                {"@currenttaskid", userActivityEntity.CurrentTaskId},
                {"@lastactive", userActivityEntity.LastActive},
                {"@timezoneoffset", userActivityEntity.TimezoneOffset},
            });

            UserActivityEntity returnRow = new UserActivityEntity();
            PropertyInfo[] properties = typeof(UserActivityEntity).GetProperties();
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
            string query = "DELETE FROM useractivity WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });



            return (result > 0 ? true : false);
        }
    }
}