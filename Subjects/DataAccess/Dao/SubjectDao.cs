using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Subjects.DataAccess.Entities;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Subjects.DataAccess.Dao
{
    public class SubjectDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public SubjectDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public SubjectEntity Get(int requestedId)
        {
            string query = "SELECT * FROM subjects WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            SubjectEntity returnRow = new SubjectEntity();

            PropertyInfo[] properties = typeof(SubjectEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<SubjectEntity> GetList(SubjectFilterRequest filter)
        {
           
            List<SubjectEntity> filterResponse = new List<SubjectEntity>();

            string query = "SELECT * FROM subjects WHERE userId = @userId and active = 1 ";

            if (filter.SemesterId > 0)
            {
                query += " AND semesterId = @SemesterId";
            }
            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object>{{"@UserId", filter.UserId}, {"@SemesterId", filter.SemesterId}});

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<SubjectEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public SubjectEntity Create(SubjectEntity subjectEntity)
        {
            string query = @"INSERT INTO subjects (
                            name,
                            classcode,
                            description,
                            professor,
                            credits,
                            userid,
                            color,
                            semesterid,
                            active) 

                            VALUES(
                            @name,
                            @classcode,
                            @description,
                            @professor,
                            @credits,
                            @userid,
                            @color,
                            @semesterid,
                            @active);  ";

            query += "SELECT * FROM subjects WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@name", subjectEntity.Name},
                {"@classcode", subjectEntity.ClassCode},
                {"@description", subjectEntity.Description},
                {"@professor", subjectEntity.Professor},
                {"@credits", subjectEntity.Credits},
                {"@userid", subjectEntity.UserId},
                {"@color", subjectEntity.Color},
                {"@semesterid", subjectEntity.SemesterId},
                {"@active", subjectEntity.Active},
            });

            SubjectEntity returnRow = new SubjectEntity();
            PropertyInfo[] properties = typeof(SubjectEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public SubjectEntity Update(SubjectEntity subjectEntity)
        {
            string query = @"UPDATE subjects SET 
                            name = @name,
                            classcode = @classcode,
                            description = @description,
                            professor = @professor,
                            credits = @credits,
                            userid = @userid,
                            color = @color,
                            semesterid = @semesterid,
                            active = @active
                            WHERE id = @id; 

                SELECT * FROM subjects WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@name", subjectEntity.Name},
                {"@classcode", subjectEntity.ClassCode},
                {"@description", subjectEntity.Description},
                {"@professor", subjectEntity.Professor},
                {"@credits", subjectEntity.Credits},
                {"@id", subjectEntity.Id},
                {"@userid", subjectEntity.UserId},
                {"@color", subjectEntity.Color},
                {"@semesterid", subjectEntity.SemesterId},
                {"@active", subjectEntity.Active},
            });

            SubjectEntity returnRow = new SubjectEntity();
            PropertyInfo[] properties = typeof(SubjectEntity).GetProperties();
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
            string query = "DELETE FROM subjects WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }
    }
}