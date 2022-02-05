using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace plannerBackEnd.Common.sqlTools
{
    public class SqlTools
    {
        private SqlConnection sqlConnection;
        private IConfiguration configuration;

        //----------------------------------------------------------------------------------------------------
        public SqlTools(
            SqlConnection sqlConnection,
            IConfiguration configuration
        )
        {
            this.sqlConnection = sqlConnection;
            this.configuration = configuration;
        }

        //-------------------------------------------------------------------------------------------------------------------
        public DataRow GetDataRow(string sql, Dictionary<string, object> parameters = null)
        {
            MySqlConnection connection = null;
            try
            {
                connection = sqlConnection.GetConnection(configuration["ServerEndpoint"]);
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataRow dataRow = null;
                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    dataRow = dataSet.Tables[0].Rows[0];
                }

                connection.Close();

                return dataRow;
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }

                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        public int Execute(string sql, Dictionary<string, object> parameters = null)
        {
            MySqlConnection connection = null;
            try
            {
                int result;
                connection = sqlConnection.GetConnection(configuration["ServerEndpoint"]);
                connection.Open();

                MySqlCommand command = new MySqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                result = command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }

                throw;
            }


        }

        //-------------------------------------------------------------------------------------------------------------------
        public object ExecuteScalar(string sql, Dictionary<string, object> parameters = null)
        {
            MySqlConnection connection = null;
            try
            {
                object result;
                connection = sqlConnection.GetConnection(configuration["ServerEndpoint"]);
                connection.Open();

                MySqlCommand command = new MySqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                result = command.ExecuteScalar();

                connection.Close();
                return result;
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }

                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        public DataTable GetTable(string sql, Dictionary<string, object> parameters = null)
        {
            MySqlConnection connection = null;
            try
            {
                string tableName = "returnTable";
                connection = sqlConnection.GetConnection(configuration["ServerEndpoint"]);
                connection.Open();

                MySqlCommand command = new MySqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, tableName);
                connection.Close();

                return dataSet.Tables[tableName];
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }

                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        public List<T> ConvertDataTable<T>(DataTable dataTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = getItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        //-------------------------------------------------------------------------------------------------------------------
        private T getItem<T>(DataRow dataRow)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                
                foreach (PropertyInfo property in temp.GetProperties())
                {
                    if (property.Name.ToLower() == column.ColumnName)
                    {
                        if (column.DataType.Name == "Int64")
                        {
                            property.SetValue(obj, Convert.ToInt32(dataRow[column.ColumnName]), null);
                        }
                        else if (column.DataType.Name == "Decimal")
                        {
                            property.SetValue(obj, Convert.ToDouble(dataRow[column.ColumnName]), null);
                        }
                        else
                        {
                            property.SetValue(obj, dataRow[column.ColumnName], null);
                        }
                    }
                    else
                        continue;

                }
            }
            return obj;
        }


    }
}