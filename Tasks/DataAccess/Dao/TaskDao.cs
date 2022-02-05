using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Tasks.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Tasks.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.DataAccess.Dao
{
    public class TaskDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public TaskDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public TaskEntity Get(int requestedId)
        {
            string query = "SELECT * FROM tasks WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);

            TaskEntity returnRow = new TaskEntity();

            PropertyInfo[] properties = typeof(TaskEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<TaskEntity> GetList(TaskFilterRequest filter)
        {

            List<TaskEntity> filterResponse = new List<TaskEntity>();

            string query = "SELECT * FROM tasks WHERE userId = @userId and active = 1 ";

            if (filter.IsDone == 0)
            {
                query += " AND isdone = 0";
            }
            if (filter.IsDone == 1)
            {
                query += " ";
            }

            query += " ORDER BY isDone";
            if (filter.FilterBySubject)
            {
                query += " , (SELECT CONCAT(name, ' ', classcode) FROM subjects WHERE id = subjectid) ASC";
            }
            else if (filter.FilterByDueDate)
            {
                query += " , duedate ASC";
            }


            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<TaskEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public TaskEntity Create(TaskEntity taskEntity)
        {
            string query = @"INSERT INTO tasks (
                            tasktype,
                            description,
                            minutes,
                            subjectid,
                            duedate,
                            userid,
                            isdone,
                            title,
                            active) 

                            VALUES(
                            @tasktype,
                            @description,
                            @minutes,
                            @subjectid,
                            @duedate,
                            @userid,
                            @isdone,
                            @title,
                            @active);  ";

            query += "SELECT * FROM tasks WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@tasktype", taskEntity.TaskType},
                {"@description", taskEntity.Description},
                {"@minutes", taskEntity.Minutes},
                {"@subjectid", taskEntity.SubjectId},
                {"@duedate", taskEntity.DueDate},
                {"@userid", taskEntity.UserId},
                {"@isdone", taskEntity.IsDone},
                {"@title", taskEntity.Title},
                {"@active", taskEntity.Active},
            });

            TaskEntity returnRow = new TaskEntity();
            PropertyInfo[] properties = typeof(TaskEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public TaskEntity Update(TaskEntity taskEntity)
        {
            string query = @"UPDATE tasks SET 
                            tasktype = @tasktype,
                            description = @description,
                            minutes = @minutes,
                            subjectid = @subjectid,
                            duedate = @duedate,
                            userid = @userid,
                            isdone = @isdone,
                            title = @title,
                            active = @active
                            WHERE id = @id; 

                SELECT * FROM tasks WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@id", taskEntity.Id},
                {"@tasktype", taskEntity.TaskType},
                {"@description", taskEntity.Description},
                {"@minutes", taskEntity.Minutes},
                {"@subjectid", taskEntity.SubjectId},
                {"@duedate", taskEntity.DueDate},
                {"@userid", taskEntity.UserId},
                {"@isdone", taskEntity.IsDone},
                {"@title", taskEntity.Title},
                {"@active", taskEntity.Active},
            });

            TaskEntity returnRow = new TaskEntity();
            PropertyInfo[] properties = typeof(TaskEntity).GetProperties();
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
            string query = "DELETE FROM tasks WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });

            return (result > 0 ? true : false);
        }
    }
}