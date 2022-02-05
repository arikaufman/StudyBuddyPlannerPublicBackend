using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Admin.DataAccess.Entities;
using plannerBackEnd.Common.sqlTools;

namespace plannerBackEnd.Admin.DataAccess.Dao
{
    public class AdminDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public AdminDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        //----------------------------------------------------------------------------------------------------
        public SmokeTestEntity CreateSmokeTest(SmokeTestEntity smokeTestEntity)
        {
            string query = @"INSERT INTO testtable (
                            description) 

                            VALUES(
                            @description);  ";

            query += "SELECT * FROM testtableWHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@description", smokeTestEntity.Description}
            });

            SmokeTestEntity returnRow = new SmokeTestEntity();
            PropertyInfo[] properties = typeof(SmokeTestEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        //----------------------------------------------------------------------------------------------------
        public void CreateErrorLogEntry(ErrorLogEntity errorLogEntity)
        {
            string query = @"INSERT INTO errorlog (
                            comments,
                            callstack,
                            user) 

                            VALUES(
                            @comments,
                            @callstack,
                            @user);  ";

            query += "SELECT * FROM errorlog WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@comments", errorLogEntity.Comments},
                {"@callstack", errorLogEntity.CallStack},
                {"@user", errorLogEntity.User}
            });

        }

        //----------------------------------------------------------------------------------------------------
        public void CreateAccessLogEntry(AccessLogEntity accessLogEntity)
        {
            string query = @"INSERT INTO accesslog (
                            httpmethod,
                            httprequest,
                            url,
                            user,
                            duration) 

                            VALUES(
                            @httpmethod,
                            @httprequest,
                            @url,
                            @user,
                            @duration);  ";

            query += "SELECT * FROM errorlog WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@httpmethod", accessLogEntity.HttpMethod},
                {"@httprequest", accessLogEntity.HttpRequest},
                {"@url", accessLogEntity.Url},
                {"@user", accessLogEntity.User },
                {"@duration", accessLogEntity.Duration }
            });

        }
    }
}
