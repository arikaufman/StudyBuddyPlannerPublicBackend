using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Semesters.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Common.Constants;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Semesters.DataAccess.Dao
{
    public class SemesterDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public SemesterDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public SemesterEntity Get(int requestedId)
        {
            string query = "SELECT * FROM semesters WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            SemesterEntity returnRow = new SemesterEntity();

            PropertyInfo[] properties = typeof(SemesterEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<SemesterEntity> GetList(SemesterFilterRequest filter)
        {

            List<SemesterEntity> filterResponse = new List<SemesterEntity>();

            string query = "SELECT * FROM semesters WHERE userId = @userId ";

            if (filter.School.Length > 0)
            {
                query += " AND (SELECT school from userprofile WHERE id = userid) = @school";
            }


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@userId", filter.UserId }, { "@school", filter.School }  });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<SemesterEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public SemesterEntity Create(SemesterEntity semesterEntity)
        {
            string query = @"INSERT INTO semesters (
                            userid,
                            startDate,  
                            endDate,
                            startgpa,
                            startpercentage,
                            active,
                            title) 

                            VALUES(
                            @userid,
                            @startDate,
                            @endDate,
                            @startgpa,
                            @startpercentage,
                            @active,
                            @title);  ";

            query += "SELECT * FROM semesters WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", semesterEntity.UserId},
                {"@startDate", semesterEntity.StartDate},
                {"@endDate", semesterEntity.EndDate},
                {"@startgpa", semesterEntity.Startgpa},
                {"@startpercentage", semesterEntity.Startpercentage},
                {"@active", semesterEntity.Active},
                {"@title", semesterEntity.Title},
            });

            SemesterEntity returnRow = new SemesterEntity();
            PropertyInfo[] properties = typeof(SemesterEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public SemesterEntity Update(SemesterEntity semesterEntity)
        {
            string query = @"UPDATE semesters SET 
                            userid = @userid,
                            startDate = @startDate,  
                            endDate = @endDate,
                            startgpa = @startgpa,
                            startpercentage = @startpercentage,
                            active = @active,
                            title = @title
                            WHERE id = @id; 

                SELECT * FROM semesters WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", semesterEntity.UserId},
                {"@startDate", semesterEntity.StartDate},
                {"@endDate", semesterEntity.EndDate},
                {"@startgpa", semesterEntity.Startgpa},
                {"@startpercentage", semesterEntity.Startpercentage},
                {"@active", semesterEntity.Active},
                {"@title", semesterEntity.Title},
                {"@id", semesterEntity.Id}
            });

            SemesterEntity returnRow = new SemesterEntity();
            PropertyInfo[] properties = typeof(SemesterEntity).GetProperties();
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
            string query = "DELETE FROM semesters WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }
    }
}