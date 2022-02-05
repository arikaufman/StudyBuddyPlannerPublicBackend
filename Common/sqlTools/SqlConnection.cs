using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading;

namespace plannerBackEnd.Common.sqlTools
{
    public class SqlConnection
    {

        //----------------------------------------------------------------------------------------------------
        public SqlConnection()
        { }

        // --------------------------------------------------------------------------------------------
        public MySqlConnection GetConnection(string server)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Database = "",
                Server = server,
                Port = 3306,
                Pooling = true,
                UserID = "",
                Password = "",
                CharacterSet = "utf8",
                SslMode = MySqlSslMode.None,
                DefaultCommandTimeout = (uint)30,
                AllowLoadLocalInfile = true
            };

            MySqlConnection connection = new MySqlConnection(connectionStringBuilder.ToString());

           // WaitForFreeConnection(connection);
            return connection;
        }

        // --------------------------------------------------------------------------------------------
        public void WaitForFreeConnection(MySqlConnection connection)
        {
            int MaxConnections = 10;
            int CurrentConnections = 0;
            connection.Open();
            do
            {
                string query = "SHOW STATUS ";
                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                DataRow threadsConnected = dataSet.Tables[0].Rows.Cast<DataRow>().FirstOrDefault(x => x["Variable_name"].ToString().Contains("Threads_connected"));
                Int32.TryParse(threadsConnected.ItemArray[1].ToString(), out CurrentConnections);
                if (CurrentConnections > MaxConnections)
                {
                    //error log 
                    Thread.Sleep(1000);
                }
            } while (CurrentConnections > MaxConnections);
            connection.Close();
        }
    }
}