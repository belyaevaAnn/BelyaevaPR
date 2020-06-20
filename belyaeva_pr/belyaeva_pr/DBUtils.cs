using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace belyaeva_pr.sqlConn
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "91.191.231.76";
            int port = 3307;
            string database = "pr_is207_3";
            string username = "pr_is207_3";
            string password = "pr_is207_3";

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
