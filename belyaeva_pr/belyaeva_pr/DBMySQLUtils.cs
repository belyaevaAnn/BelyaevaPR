using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace belyaeva_pr.sqlConn
{
    class DBMySQLUtils
    {
        public static MySqlConnection
        GetDBConnection(string host, int port, string database, string username, string password)
        {
            String connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }
}
