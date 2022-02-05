using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Schools.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace plannerBackEnd.Schools.DataAccess.Dao
{
    public class SchoolDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public SchoolDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public SchoolEntity Get(int requestedId)
        {
            string query = "SELECT * FROM schools S WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            SchoolEntity returnRow = new SchoolEntity();

            PropertyInfo[] properties = typeof(SchoolEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "NumberOfStudents")
                {
                    property.SetValue(returnRow, dataRow.ItemArray[i]);
                    i++;
                }
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<SchoolEntity> GetList()
        {
            string query = @"SELECT *, CAST(ROUND((SELECT COUNT(*) FROM userprofile WHERE schoolId = S.id),0) AS double) AS `numberofstudents`
                                FROM schools S ORDER BY (SELECT COUNT(*) FROM userprofile WHERE schoolId = S.id) DESC ";

            List<SchoolEntity> filterResponse = new List<SchoolEntity>();


            DataTable dataTable = sqlTools.GetTable(query);

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<SchoolEntity>(dataTable);
            }

            return filterResponse;
        }
    }
}