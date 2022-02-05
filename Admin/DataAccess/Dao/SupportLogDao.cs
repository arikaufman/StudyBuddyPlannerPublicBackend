using plannerBackEnd.Admin.DataAccess.Entities;
using plannerBackEnd.Common.sqlTools;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace plannerBackEnd.Admin.DataAccess.Dao
{
    public class SupportLogDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public SupportLogDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public SupportLogEntity Get(int requestedId)
        {
            string query = "SELECT * FROM supportlog WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            SupportLogEntity returnRow = new SupportLogEntity();

            PropertyInfo[] properties = typeof(SupportLogEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<SupportLogEntity> GetList()
        {

            List<SupportLogEntity> filterResponse = new List<SupportLogEntity>();

            string query = "SELECT * FROM supportlog";


            DataTable dataTable = sqlTools.GetTable(query);

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<SupportLogEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public SupportLogEntity Create(SupportLogEntity supportLogEntity)
        {
            string query = @"INSERT INTO supportlog (
                            userid,
                            useremail,  
                            description,
                            priority,
                            requesttype,
                            status) 

                            VALUES(
                            @userid,
                            @useremail,
                            @description,
                            @priority,
                            @requesttype,
                            @status);  ";

            query += "SELECT * FROM supportlog WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", supportLogEntity.UserId},
                {"@useremail", supportLogEntity.UserEmail},
                {"@description", supportLogEntity.Description},
                {"@priority", supportLogEntity.Priority},
                {"@requesttype", supportLogEntity.RequestType},
                {"@status", supportLogEntity.Status},
            });

            SupportLogEntity returnRow = new SupportLogEntity();
            PropertyInfo[] properties = typeof(SupportLogEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public SupportLogEntity Update(SupportLogEntity supportLogEntity)
        {
            string query = @"UPDATE supportlog SET 
                            userid = @userid,
                            useremail = @useremail,  
                            description = @description,
                            priority = @priority,
                            requesttype = @requesttype,
                            status = @status
                            WHERE id = @id; 

                SELECT * FROM supportlog WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", supportLogEntity.UserId},
                {"@useremail", supportLogEntity.UserEmail},
                {"@description", supportLogEntity.Description},
                {"@priority", supportLogEntity.Priority},
                {"@requesttype", supportLogEntity.RequestType},
                {"@status", supportLogEntity.Status},
                {"@id", supportLogEntity.Id},
            });

            SupportLogEntity returnRow = new SupportLogEntity();
            PropertyInfo[] properties = typeof(SupportLogEntity).GetProperties();
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
            string query = "DELETE FROM supportlog WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }
    }
}