using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportData
{
    public class DataAccess
    {
        private string _strConnection;

        public DataAccess()
        {
            var host = Properties.Settings.Default.DbHost;
            var port = "3306";
            var schema = "MTGC";
            var user = Properties.Settings.Default.DbUser;
            var pass = Properties.Settings.Default.DbPass;

            _strConnection = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", host, port, schema, user, pass);
        }

        public bool CheckDbConnection()
        {
            return true;
        }

        public int CountSets()
        {
            var result = 0;

            try
            {
                var sql = "select count(id) as SetCounter from sets";

                using (MySqlConnection conn = new MySqlConnection(_strConnection))
                {
                    conn.Open();
                    MySqlCommand comm = new MySqlCommand(sql, conn);
                    result = Convert.ToInt32(comm.ExecuteScalar());
                }
            }
            catch(Exception)
            {
                result = -1;
            }
            return result;
        }
    }
}
