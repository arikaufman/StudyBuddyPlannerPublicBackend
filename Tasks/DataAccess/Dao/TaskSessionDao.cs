using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Tasks.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace plannerBackEnd.Tasks.DataAccess.Dao
{
    public class TaskSessionDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public TaskSessionDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public TaskSessionEntity Get(int requestedId)
        {
            string query = "SELECT * FROM tasksessions WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);

            TaskSessionEntity returnRow = new TaskSessionEntity();

            PropertyInfo[] properties = typeof(TaskSessionEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public List<TaskSessionEntity> GetList(int taskId)
        {

            List<TaskSessionEntity> filterResponse = new List<TaskSessionEntity>();

            string query = "SELECT * FROM tasksessions WHERE taskId = @taskId ORDER BY datecompleted ASC";



            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@taskId", taskId } });

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<TaskSessionEntity>(dataTable);
            }

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public TaskSessionEntity Create(TaskSessionEntity taskSessionEntity)
        {
            string query = @"INSERT INTO tasksessions (
                            minutes,
                            dateCompleted,
                            taskId,
                            title) 

                            VALUES(
                            @minutes,
                            @dateCompleted,
                            @taskId,
                            @title);  ";

            query += "SELECT * FROM tasksessions WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@minutes", taskSessionEntity.Minutes},
                {"@dateCompleted", taskSessionEntity.DateCompleted},
                {"@taskId", taskSessionEntity.TaskId},
                {"@title", taskSessionEntity.Title}
            });

            TaskSessionEntity returnRow = new TaskSessionEntity();
            PropertyInfo[] properties = typeof(TaskSessionEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            updateTaskTime(taskSessionEntity.TaskId);
            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public TaskSessionEntity Update(TaskSessionEntity taskSessionEntity)
        {
            string query = @"UPDATE tasksessions SET 
                            minutes = @minutes,
                            dateCompleted = @dateCompleted,
                            taskId = @taskId,
                            title = @title
                            WHERE id = @id; 

                SELECT * FROM tasksessions WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@id", taskSessionEntity.Id},
                {"@minutes", taskSessionEntity.Minutes},
                {"@dateCompleted", taskSessionEntity.DateCompleted},
                {"@taskId", taskSessionEntity.TaskId},
                {"@title", taskSessionEntity.Title}
            });

            TaskSessionEntity returnRow = new TaskSessionEntity();
            PropertyInfo[] properties = typeof(TaskSessionEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }
            updateTaskTime(taskSessionEntity.TaskId);
            return returnRow;
        }

        // --------------------------------------------------------------------------------------------
        public bool Delete(int requestedId)
        {
            TaskSessionEntity taskSessionEntity = Get(requestedId);

            string query = "DELETE FROM tasksessions WHERE id = @id";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@id", requestedId } });
            if (result > 0)
            {
                updateTaskTime(taskSessionEntity.TaskId);
            }
            return (result > 0 ? true : false);
        }

        // --------------------------------------------------------------------------------------------

        public bool updateTaskTime(int taskId)
        {
            string query = "UPDATE tasks SET minutes = IFNULL((SELECT SUM(minutes) FROM tasksessions WHERE taskid = @taskId),0) WHERE id = @taskId";

            int result = sqlTools.Execute(query, new Dictionary<string, object> { { "@taskId", taskId }});

            return (result > 0 ? true : false);
        }

    }
}