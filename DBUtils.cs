using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class DBUtils
    {

        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "mydb";
            string username = "monty";
            string password = "Some_pass";

            return DBMSQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}